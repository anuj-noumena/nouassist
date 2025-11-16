import 'package:flutter/material.dart';
import '../../../../core/theme/app_colors.dart';
import '../../../../core/theme/app_spacing.dart';
import '../../../../data/models/project_dto.dart';
import '../../../atomic/app_text.dart';

class ProjectTreeNode extends StatefulWidget {
  final ProjectDto project;
  final List<ProjectDto> children;
  final int level;
  final VoidCallback? onTap;
  final VoidCallback? onEdit;
  final VoidCallback? onDelete;

  const ProjectTreeNode({
    super.key,
    required this.project,
    required this.children,
    this.level = 0,
    this.onTap,
    this.onEdit,
    this.onDelete,
  });

  @override
  State<ProjectTreeNode> createState() => _ProjectTreeNodeState();
}

class _ProjectTreeNodeState extends State<ProjectTreeNode> {
  bool _isExpanded = true;

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Container(
          margin: EdgeInsets.only(left: widget.level * AppSpacing.xl),
          child: Card(
            margin: const EdgeInsets.symmetric(vertical: AppSpacing.xs),
            child: InkWell(
              onTap: widget.onTap,
              borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
              child: Padding(
                padding: const EdgeInsets.all(AppSpacing.md),
                child: Row(
                  children: [
                    if (widget.children.isNotEmpty)
                      IconButton(
                        icon: Icon(
                          _isExpanded ? Icons.expand_more : Icons.chevron_right,
                        ),
                        onPressed: () {
                          setState(() {
                            _isExpanded = !_isExpanded;
                          });
                        },
                        padding: EdgeInsets.zero,
                        constraints: const BoxConstraints(),
                      )
                    else
                      const SizedBox(width: 24),
                    const SizedBox(width: AppSpacing.sm),
                    Container(
                      width: 4,
                      height: 40,
                      decoration: BoxDecoration(
                        color: Color(int.parse(
                            widget.project.color.replaceFirst('#', '0xFF'))),
                        borderRadius: BorderRadius.circular(2),
                      ),
                    ),
                    const SizedBox(width: AppSpacing.md),
                    Expanded(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          AppText(
                            widget.project.name,
                            variant: TextVariant.bodyMedium,
                            fontWeight: FontWeight.w600,
                          ),
                          const SizedBox(height: AppSpacing.xs),
                          Row(
                            children: [
                              _buildStatusBadge(),
                              const SizedBox(width: AppSpacing.sm),
                              AppText(
                                '${widget.project.progress.toInt()}%',
                                variant: TextVariant.bodySmall,
                                color: AppColors.textSecondary,
                              ),
                            ],
                          ),
                        ],
                      ),
                    ),
                    PopupMenuButton<String>(
                      onSelected: (value) {
                        if (value == 'edit') {
                          widget.onEdit?.call();
                        } else if (value == 'delete') {
                          widget.onDelete?.call();
                        }
                      },
                      itemBuilder: (context) => [
                        const PopupMenuItem(
                          value: 'edit',
                          child: Row(
                            children: [
                              Icon(Icons.edit, size: 20),
                              SizedBox(width: 8),
                              Text('Edit'),
                            ],
                          ),
                        ),
                        const PopupMenuItem(
                          value: 'delete',
                          child: Row(
                            children: [
                              Icon(Icons.delete, size: 20, color: AppColors.error),
                              SizedBox(width: 8),
                              Text('Delete',
                                  style: TextStyle(color: AppColors.error)),
                            ],
                          ),
                        ),
                      ],
                    ),
                  ],
                ),
              ),
            ),
          ),
        ),
        if (_isExpanded && widget.children.isNotEmpty)
          ...widget.children.map((child) {
            return ProjectTreeNode(
              project: child,
              children: const [], // For simplicity, only showing 2 levels
              level: widget.level + 1,
              onTap: widget.onTap,
              onEdit: widget.onEdit,
              onDelete: widget.onDelete,
            );
          }).toList(),
      ],
    );
  }

  Widget _buildStatusBadge() {
    return Container(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.sm,
        vertical: AppSpacing.xxs,
      ),
      decoration: BoxDecoration(
        color: _getStatusColor().withOpacity(0.1),
        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
      ),
      child: Text(
        widget.project.status.displayName,
        style: TextStyle(
          fontSize: 10,
          fontWeight: FontWeight.w600,
          color: _getStatusColor(),
        ),
      ),
    );
  }

  Color _getStatusColor() {
    switch (widget.project.status) {
      case ProjectStatus.planning:
        return AppColors.info;
      case ProjectStatus.active:
        return AppColors.success;
      case ProjectStatus.onHold:
        return AppColors.warning;
      case ProjectStatus.completed:
        return AppColors.success;
      case ProjectStatus.cancelled:
        return AppColors.error;
    }
  }
}
