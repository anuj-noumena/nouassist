// src/NouAssist.CoreProductivity.ProjectManagement.Domain/Services/ProjectDependencyManagementService.cs

using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.ProjectManagement.Services
{
    public class ProjectDependencyManagementService : DomainService
    {
        /// <summary>
        /// Detects circular dependencies in project dependency chain
        /// </summary>
        public DependencyValidationResult ValidateDependencyChain(
            Guid projectId,
            Guid dependsOnProjectId,
            List<ProjectDependency> allDependencies)
        {
            var result = new DependencyValidationResult { IsValid = true };

            // Check for self-dependency
            if (projectId == dependsOnProjectId)
            {
                result.IsValid = false;
                result.Errors.Add("Project cannot depend on itself.");
                return result;
            }

            // Check for circular dependencies
            var visited = new HashSet<Guid>();
            var stack = new Stack<Guid>();

            if (HasCircularDependency(projectId, dependsOnProjectId, allDependencies, visited, stack))
            {
                result.IsValid = false;
                result.Errors.Add($"Circular dependency detected: {string.Join(" -> ", stack.Reverse())}");
            }

            return result;
        }

        private bool HasCircularDependency(
            Guid startProjectId,
            Guid currentProjectId,
            List<ProjectDependency> allDependencies,
            HashSet<Guid> visited,
            Stack<Guid> stack)
        {
            if (currentProjectId == startProjectId && visited.Any())
            {
                stack.Push(currentProjectId);
                return true;
            }

            if (visited.Contains(currentProjectId))
            {
                return false;
            }

            visited.Add(currentProjectId);
            stack.Push(currentProjectId);

            var dependencies = allDependencies
                .Where(d => d.ProjectId == currentProjectId && d.IsActive)
                .ToList();

            foreach (var dep in dependencies)
            {
                if (HasCircularDependency(startProjectId, dep.DependsOnProjectId, allDependencies, visited, stack))
                {
                    return true;
                }
            }

            stack.Pop();
            return false;
        }

        /// <summary>
        /// Calculates the critical path for project dependencies
        /// </summary>
        public CriticalPathResult CalculateCriticalPath(
            List<ProjectDependency> dependencies,
            Dictionary<Guid, int> projectDurations)
        {
            var result = new CriticalPathResult();
            
            // Topological sort and calculate earliest start times
            var graph = BuildDependencyGraph(dependencies);
            var sorted = TopologicalSort(graph);

            var earliestStart = new Dictionary<Guid, int>();
            var earliestFinish = new Dictionary<Guid, int>();

            foreach (var projectId in sorted)
            {
                var duration = projectDurations.GetValueOrDefault(projectId, 0);
                var maxPredecessorFinish = 0;

                var predecessors = dependencies
                    .Where(d => d.DependsOnProjectId == projectId)
                    .Select(d => d.ProjectId);

                foreach (var pred in predecessors)
                {
                    if (earliestFinish.ContainsKey(pred))
                    {
                        var dep = dependencies.First(d => d.ProjectId == pred && d.DependsOnProjectId == projectId);
                        maxPredecessorFinish = Math.Max(maxPredecessorFinish, 
                            earliestFinish[pred] + dep.LagDays);
                    }
                }

                earliestStart[projectId] = maxPredecessorFinish;
                earliestFinish[projectId] = maxPredecessorFinish + duration;
            }

            result.ProjectEarliestStarts = earliestStart;
            result.TotalDuration = earliestFinish.Values.Any() ? earliestFinish.Values.Max() : 0;

            return result;
        }

        private Dictionary<Guid, List<Guid>> BuildDependencyGraph(List<ProjectDependency> dependencies)
        {
            var graph = new Dictionary<Guid, List<Guid>>();

            foreach (var dep in dependencies.Where(d => d.IsActive))
            {
                if (!graph.ContainsKey(dep.ProjectId))
                {
                    graph[dep.ProjectId] = new List<Guid>();
                }

                graph[dep.ProjectId].Add(dep.DependsOnProjectId);
            }

            return graph;
        }

        private List<Guid> TopologicalSort(Dictionary<Guid, List<Guid>> graph)
        {
            var result = new List<Guid>();
            var visited = new HashSet<Guid>();
            var temp = new HashSet<Guid>();

            void Visit(Guid node)
            {
                if (temp.Contains(node))
                {
                    throw new BusinessException("Projects:CircularDependency");
                }

                if (!visited.Contains(node))
                {
                    temp.Add(node);

                    if (graph.ContainsKey(node))
                    {
                        foreach (var neighbor in graph[node])
                        {
                            Visit(neighbor);
                        }
                    }

                    temp.Remove(node);
                    visited.Add(node);
                    result.Insert(0, node);
                }
            }

            foreach (var node in graph.Keys)
            {
                Visit(node);
            }

            return result;
        }
    }

    public class DependencyValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    public class CriticalPathResult
    {
        public Dictionary<Guid, int> ProjectEarliestStarts { get; set; } = new();
        public int TotalDuration { get; set; }
    }
}