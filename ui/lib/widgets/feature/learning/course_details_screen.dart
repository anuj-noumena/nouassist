import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/course_dto.dart';
import '../../../providers/course_provider.dart';
import '../../../providers/lesson_provider.dart';
import '../../../providers/progress_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_button.dart';
import '../../atomic/app_loading_indicator.dart';
import '../../atomic/app_error_widget.dart';
import 'widgets/lesson_card.dart';
import 'widgets/progress_widget.dart';
import 'lesson_viewer_screen.dart';

class CourseDetailsScreen extends ConsumerWidget {
  final String courseId;

  const CourseDetailsScreen({super.key, required this.courseId});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final courseAsync = ref.watch(courseProvider(courseId));
    final lessonsAsync = ref.watch(courseLessonsProvider(courseId));
    final progressAsync = ref.watch(courseProgressProvider('user-1', courseId));

    // Changed: We removed the outer Scaffold.
    // Now we return the specific Scaffold based on the state (data/loading/error).
    return courseAsync.when(
      data: (course) => _buildCourseDetails(
        context,
        ref,
        course,
        lessonsAsync,
        progressAsync,
      ),
      loading: () => const Scaffold(body: AppLoadingIndicator()),
      error: (error, _) =>
          Scaffold(body: AppErrorWidget(error: error.toString())),
    );
  }

  Widget _buildCourseDetails(
    BuildContext context,
    WidgetRef ref,
    CourseDto course,
    AsyncValue lessonsAsync,
    AsyncValue progressAsync,
  ) {
    // Changed: This now returns a Scaffold, which supports bottomNavigationBar
    return Scaffold(
      body: CustomScrollView(
        slivers: [
          // App bar with course image
          SliverAppBar(
            expandedHeight: 250,
            pinned: true,
            flexibleSpace: FlexibleSpaceBar(
              background: course.thumbnail != null
                  ? Image.network(
                      course.thumbnail!,
                      fit: BoxFit.cover,
                      errorBuilder: (_, __, ___) => _buildPlaceholder(),
                    )
                  : _buildPlaceholder(),
            ),
          ),
          // Course content
          SliverToBoxAdapter(
            child: Padding(
              padding: const EdgeInsets.all(AppSpacing.md),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // Category badge
                  Container(
                    padding: const EdgeInsets.symmetric(
                      horizontal: AppSpacing.sm,
                      vertical: AppSpacing.xs,
                    ),
                    decoration: BoxDecoration(
                      color: AppColors.primary.withOpacity(0.1),
                      borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                    ),
                    child: Text(
                      course.category.displayName,
                      style: const TextStyle(
                        fontSize: 12,
                        fontWeight: FontWeight.w600,
                        color: AppColors.primary,
                      ),
                    ),
                  ),
                  const SizedBox(height: AppSpacing.md),
                  // Title
                  AppText(
                    course.title,
                    variant: TextVariant.h3,
                    fontWeight: FontWeight.bold,
                  ),
                  const SizedBox(height: AppSpacing.sm),
                  // Instructor
                  Row(
                    children: [
                      CircleAvatar(
                        radius: 16,
                        backgroundColor: AppColors.primary,
                        child: Text(
                          course.instructorName[0],
                          style: const TextStyle(color: Colors.white),
                        ),
                      ),
                      const SizedBox(width: AppSpacing.sm),
                      AppText(
                        course.instructorName,
                        variant: TextVariant.bodyMedium,
                        fontWeight: FontWeight.w500,
                      ),
                    ],
                  ),
                  const SizedBox(height: AppSpacing.md),
                  // Stats
                  Row(
                    children: [
                      Icon(Icons.star, size: 20, color: AppColors.warning),
                      const SizedBox(width: AppSpacing.xs),
                      AppText(
                        course.rating.toStringAsFixed(1),
                        variant: TextVariant.bodyMedium,
                        fontWeight: FontWeight.w600,
                      ),
                      const SizedBox(width: AppSpacing.xs),
                      AppText(
                        '(${course.reviewCount} reviews)',
                        variant: TextVariant.bodySmall,
                        color: AppColors.textSecondary,
                      ),
                      const SizedBox(width: AppSpacing.md),
                      Icon(
                        Icons.people_outline,
                        size: 20,
                        color: AppColors.textSecondary,
                      ),
                      const SizedBox(width: AppSpacing.xs),
                      AppText(
                        '${course.enrollmentCount} students',
                        variant: TextVariant.bodySmall,
                        color: AppColors.textSecondary,
                      ),
                    ],
                  ),
                  const SizedBox(height: AppSpacing.lg),
                  // Description
                  AppText(
                    'About this course',
                    variant: TextVariant.h5,
                    fontWeight: FontWeight.w600,
                  ),
                  const SizedBox(height: AppSpacing.sm),
                  AppText(
                    course.description,
                    variant: TextVariant.bodyMedium,
                    color: AppColors.textSecondary,
                  ),
                  const SizedBox(height: AppSpacing.lg),
                  // Course info
                  _buildInfoRow(
                    Icons.signal_cellular_alt,
                    'Level',
                    course.difficulty.displayName,
                  ),
                  _buildInfoRow(
                    Icons.access_time,
                    'Duration',
                    '${course.estimatedDuration ~/ 60} hours',
                  ),
                  _buildInfoRow(
                    Icons.play_circle_outline,
                    'Lessons',
                    '${course.lessonCount} lessons',
                  ),
                  if (course.isCertified)
                    _buildInfoRow(
                      Icons.verified,
                      'Certificate',
                      'Certificate of completion',
                    ),
                  const SizedBox(height: AppSpacing.lg),
                  // Tags
                  if (course.tags.isNotEmpty) ...[
                    AppText(
                      'What you\'ll learn',
                      variant: TextVariant.h5,
                      fontWeight: FontWeight.w600,
                    ),
                    const SizedBox(height: AppSpacing.sm),
                    Wrap(
                      spacing: AppSpacing.sm,
                      runSpacing: AppSpacing.sm,
                      children: course.tags.map((tag) {
                        return Chip(
                          label: Text(tag),
                          backgroundColor: AppColors.surfaceVariant,
                        );
                      }).toList(),
                    ),
                    const SizedBox(height: AppSpacing.lg),
                  ],
                  // Progress widget
                  progressAsync.when(
                    data: (progress) {
                      if (progress != null) {
                        return Column(
                          children: [
                            ProgressWidget(
                              percentage: progress.completionPercentage,
                              completedLessons: progress.completedLessons,
                              totalLessons: progress.totalLessons,
                              timeSpent: progress.timeSpent,
                            ),
                            const SizedBox(height: AppSpacing.lg),
                          ],
                        );
                      }
                      return const SizedBox.shrink();
                    },
                    loading: () => const SizedBox.shrink(),
                    error: (_, __) => const SizedBox.shrink(),
                  ),
                  // Course content
                  AppText(
                    'Course Content',
                    variant: TextVariant.h5,
                    fontWeight: FontWeight.w600,
                  ),
                  const SizedBox(height: AppSpacing.md),
                  lessonsAsync.when(
                    data: (lessons) {
                      return Column(
                        children: lessons.map<Widget>((lesson) {
                          return LessonCard(
                            lesson: lesson,
                            isCompleted: false,
                            isLocked:
                                !lesson.isFree && progressAsync.value == null,
                            onTap: () {
                              if (lesson.isFree ||
                                  progressAsync.value != null) {
                                Navigator.of(context).push(
                                  MaterialPageRoute(
                                    builder: (_) => LessonViewerScreen(
                                      lessonId: lesson.id,
                                      courseId: courseId,
                                    ),
                                  ),
                                );
                              }
                            },
                          );
                        }).toList(),
                      );
                    },
                    loading: () => const AppLoadingIndicator(),
                    error: (error, _) => Text('Error loading lessons: $error'),
                  ),
                  // Add padding at bottom so content isn't hidden by the bottom bar
                  const SizedBox(height: AppSpacing.xl),
                ],
              ),
            ),
          ),
        ],
      ),
      bottomNavigationBar: progressAsync.when(
        data: (progress) {
          if (progress == null) {
            return _buildEnrollButton(context, ref, course);
          }
          return null;
        },
        loading: () => null,
        error: (_, __) => null,
      ),
    );
  }

  Widget _buildPlaceholder() {
    return Container(
      color: AppColors.surfaceVariant,
      child: Center(
        child: Icon(Icons.school, size: 64, color: AppColors.textDisabled),
      ),
    );
  }

  Widget _buildInfoRow(IconData icon, String label, String value) {
    return Padding(
      padding: const EdgeInsets.only(bottom: AppSpacing.sm),
      child: Row(
        children: [
          Icon(icon, size: 20, color: AppColors.textSecondary),
          const SizedBox(width: AppSpacing.sm),
          AppText(
            '$label:',
            variant: TextVariant.bodyMedium,
            color: AppColors.textSecondary,
          ),
          const SizedBox(width: AppSpacing.sm),
          AppText(
            value,
            variant: TextVariant.bodyMedium,
            fontWeight: FontWeight.w600,
          ),
        ],
      ),
    );
  }

  Widget _buildEnrollButton(
    BuildContext context,
    WidgetRef ref,
    CourseDto course,
  ) {
    return Container(
      padding: const EdgeInsets.all(AppSpacing.md),
      decoration: BoxDecoration(
        color: AppColors.surface,
        boxShadow: [
          BoxShadow(
            color: Colors.black.withOpacity(0.1),
            blurRadius: 10,
            offset: const Offset(0, -2),
          ),
        ],
      ),
      child: SafeArea(
        child: Row(
          children: [
            if (!course.isFree && course.price != null)
              Column(
                mainAxisSize: MainAxisSize.min,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  AppText(
                    '\$${course.price!.toStringAsFixed(2)}',
                    variant: TextVariant.h4,
                    fontWeight: FontWeight.bold,
                    color: AppColors.primary,
                  ),
                  AppText(
                    'One-time payment',
                    variant: TextVariant.bodySmall,
                    color: AppColors.textSecondary,
                  ),
                ],
              )
            else
              const Column(
                mainAxisSize: MainAxisSize.min,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    'FREE',
                    style: TextStyle(
                      fontSize: 24,
                      fontWeight: FontWeight.bold,
                      color: AppColors.success,
                    ),
                  ),
                  Text(
                    'Free course',
                    style: TextStyle(
                      fontSize: 12,
                      color: AppColors.textSecondary,
                    ),
                  ),
                ],
              ),
            const SizedBox(width: AppSpacing.md),
            Expanded(
              child: AppButton(
                label: 'Enroll Now',
                onPressed: () async {
                  await ref.read(
                    updateProgressProvider(
                      'user-1',
                      courseId,
                      completedLessons: 0,
                      totalLessons: course.lessonCount,
                      timeSpent: 0,
                    ).future,
                  );

                  if (context.mounted) {
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(
                        content: Text('Successfully enrolled in course!'),
                        backgroundColor: AppColors.success,
                      ),
                    );
                  }
                },
              ),
            ),
          ],
        ),
      ),
    );
  }
}
