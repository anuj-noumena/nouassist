import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/course_dto.dart';
import '../../../providers/course_provider.dart';
import '../../atomic/app_loading_indicator.dart';
import '../../atomic/app_error_widget.dart';
import '../../composite/app_pagination_bar.dart';
import 'widgets/course_card.dart';
import 'course_details_screen.dart';

class CourseListScreen extends ConsumerStatefulWidget {
  const CourseListScreen({super.key});

  @override
  ConsumerState<CourseListScreen> createState() => _CourseListScreenState();
}

class _CourseListScreenState extends ConsumerState<CourseListScreen> {
  final _searchController = TextEditingController();
  CourseCategory? _selectedCategory;
  CourseDifficulty? _selectedDifficulty;

  @override
  void dispose() {
    _searchController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final coursesAsync = ref.watch(courseListProvider);

    return Scaffold(
      appBar: AppBar(
        title: const Text('Explore Courses'),
      ),
      body: Column(
        children: [
          // Search and filters
          Padding(
            padding: const EdgeInsets.all(AppSpacing.md),
            child: Column(
              children: [
                // Search bar
                TextField(
                  controller: _searchController,
                  decoration: InputDecoration(
                    hintText: 'Search courses...',
                    prefixIcon: const Icon(Icons.search),
                    suffixIcon: _searchController.text.isNotEmpty
                        ? IconButton(
                            icon: const Icon(Icons.clear),
                            onPressed: () {
                              _searchController.clear();
                              ref
                                  .read(courseListProvider.notifier)
                                  .search(null);
                            },
                          )
                        : null,
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
                    ),
                  ),
                  onChanged: (value) {
                    ref
                        .read(courseListProvider.notifier)
                        .search(value.isEmpty ? null : value);
                  },
                ),
                const SizedBox(height: AppSpacing.md),
                // Filters
                SingleChildScrollView(
                  scrollDirection: Axis.horizontal,
                  child: Row(
                    children: [
                      // Category filter
                      FilterChip(
                        label: Text(_selectedCategory?.displayName ?? 'All Categories'),
                        selected: _selectedCategory != null,
                        onSelected: (_) {
                          _showCategoryFilter();
                        },
                      ),
                      const SizedBox(width: AppSpacing.sm),
                      // Difficulty filter
                      FilterChip(
                        label: Text(_selectedDifficulty?.displayName ?? 'All Levels'),
                        selected: _selectedDifficulty != null,
                        onSelected: (_) {
                          _showDifficultyFilter();
                        },
                      ),
                      if (_selectedCategory != null || _selectedDifficulty != null) ...[
                        const SizedBox(width: AppSpacing.sm),
                        TextButton(
                          onPressed: _clearFilters,
                          child: const Text('Clear Filters'),
                        ),
                      ],
                    ],
                  ),
                ),
              ],
            ),
          ),
          const Divider(),
          // Course list
          Expanded(
            child: coursesAsync.when(
              data: (pagedResult) {
                if (pagedResult.items.isEmpty) {
                  return Center(
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Icon(
                          Icons.search_off,
                          size: 64,
                          color: AppColors.textDisabled,
                        ),
                        const SizedBox(height: AppSpacing.md),
                        const Text(
                          'No courses found',
                          style: TextStyle(
                            fontSize: 18,
                            color: AppColors.textSecondary,
                          ),
                        ),
                      ],
                    ),
                  );
                }

                return ListView.builder(
                  padding: const EdgeInsets.all(AppSpacing.md),
                  itemCount: pagedResult.items.length,
                  itemBuilder: (context, index) {
                    final course = pagedResult.items[index];
                    return CourseCard(
                      course: course,
                      onTap: () {
                        Navigator.of(context).push(
                          MaterialPageRoute(
                            builder: (_) => CourseDetailsScreen(
                              courseId: course.id,
                            ),
                          ),
                        );
                      },
                    );
                  },
                );
              },
              loading: () => const AppLoadingIndicator(),
              error: (error, _) => AppErrorWidget(
                error: error.toString(),
                onRetry: () => ref.invalidate(courseListProvider),
              ),
            ),
          ),
          // Pagination
          if (coursesAsync.hasValue && coursesAsync.value!.items.isNotEmpty)
            AppPaginationBar(
              currentPage: 1,
              totalPages: (coursesAsync.value!.totalCount / 10).ceil(),
              onPreviousPage: () {
                ref.read(courseListProvider.notifier).previousPage();
              },
              onNextPage: () {
                ref.read(courseListProvider.notifier).nextPage();
              },
            ),
        ],
      ),
    );
  }

  void _showCategoryFilter() {
    showModalBottomSheet(
      context: context,
      builder: (context) => ListView(
        shrinkWrap: true,
        children: [
          ListTile(
            title: const Text('All Categories'),
            trailing: _selectedCategory == null
                ? const Icon(Icons.check, color: AppColors.primary)
                : null,
            onTap: () {
              setState(() {
                _selectedCategory = null;
              });
              ref.read(courseListProvider.notifier).setCategory(null);
              Navigator.pop(context);
            },
          ),
          ...CourseCategory.values.map((category) {
            return ListTile(
              title: Text(category.displayName),
              trailing: _selectedCategory == category
                  ? const Icon(Icons.check, color: AppColors.primary)
                  : null,
              onTap: () {
                setState(() {
                  _selectedCategory = category;
                });
                ref.read(courseListProvider.notifier).setCategory(category);
                Navigator.pop(context);
              },
            );
          }).toList(),
        ],
      ),
    );
  }

  void _showDifficultyFilter() {
    showModalBottomSheet(
      context: context,
      builder: (context) => ListView(
        shrinkWrap: true,
        children: [
          ListTile(
            title: const Text('All Levels'),
            trailing: _selectedDifficulty == null
                ? const Icon(Icons.check, color: AppColors.primary)
                : null,
            onTap: () {
              setState(() {
                _selectedDifficulty = null;
              });
              ref.read(courseListProvider.notifier).setDifficulty(null);
              Navigator.pop(context);
            },
          ),
          ...CourseDifficulty.values.map((difficulty) {
            return ListTile(
              title: Text(difficulty.displayName),
              trailing: _selectedDifficulty == difficulty
                  ? const Icon(Icons.check, color: AppColors.primary)
                  : null,
              onTap: () {
                setState(() {
                  _selectedDifficulty = difficulty;
                });
                ref.read(courseListProvider.notifier).setDifficulty(difficulty);
                Navigator.pop(context);
              },
            );
          }).toList(),
        ],
      ),
    );
  }

  void _clearFilters() {
    setState(() {
      _selectedCategory = null;
      _selectedDifficulty = null;
    });
    ref.read(courseListProvider.notifier).setCategory(null);
    ref.read(courseListProvider.notifier).setDifficulty(null);
  }
}
