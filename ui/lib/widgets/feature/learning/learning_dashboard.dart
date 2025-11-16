import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:ui/data/models/learning_progress_dto.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../providers/course_provider.dart';
import '../../../providers/progress_provider.dart';
import '../../../providers/certificate_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_loading_indicator.dart';
import '../../atomic/app_button.dart';
import 'widgets/course_card.dart';
import 'course_list_screen.dart';
import 'course_details_screen.dart';
import 'certificates_screen.dart';

class LearningDashboard extends ConsumerWidget {
  const LearningDashboard({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final featuredCoursesAsync = ref.watch(featuredCoursesProvider);
    final progressAsync = ref.watch(userCourseProgressProvider('user-1'));
    final certificatesAsync = ref.watch(userCertificatesProvider('user-1'));

    return Scaffold(
      appBar: AppBar(
        title: const Text('Learning Dashboard'),
        actions: [
          IconButton(
            icon: const Icon(Icons.search),
            onPressed: () {
              Navigator.of(context).push(
                MaterialPageRoute(
                  builder: (_) => const CourseListScreen(),
                ),
              );
            },
          ),
        ],
      ),
      body: RefreshIndicator(
        onRefresh: () async {
          ref.invalidate(featuredCoursesProvider);
          ref.invalidate(userCourseProgressProvider);
          ref.invalidate(userCertificatesProvider);
        },
        child: SingleChildScrollView(
          padding: const EdgeInsets.all(AppSpacing.md),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              // Welcome section
              _buildWelcomeSection(context),
              const SizedBox(height: AppSpacing.xl),

              // Stats cards
              _buildStatsCards(progressAsync, certificatesAsync),
              const SizedBox(height: AppSpacing.xl),

              // Continue learning section
              progressAsync.when(
                data: (progress) {
                  final inProgress = progress
                      .where((p) => p.status == ProgressStatus.inProgress)
                      .toList();

                  if (inProgress.isNotEmpty) {
                    return Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            AppText(
                              'Continue Learning',
                              variant: TextVariant.h4,
                              fontWeight: FontWeight.bold,
                            ),
                            TextButton(
                              onPressed: () {
                                Navigator.of(context).push(
                                  MaterialPageRoute(
                                    builder: (_) => const CourseListScreen(),
                                  ),
                                );
                              },
                              child: const Text('View All'),
                            ),
                          ],
                        ),
                        const SizedBox(height: AppSpacing.md),
                        ...inProgress.take(2).map((courseProgress) {
                          return FutureBuilder(
                            future: ref.read(
                              courseProvider(courseProgress.courseId).future,
                            ),
                            builder: (context, snapshot) {
                              if (snapshot.hasData) {
                                return CourseCard(
                                  course: snapshot.data!,
                                  progress: courseProgress.completionPercentage,
                                  onTap: () {
                                    Navigator.of(context).push(
                                      MaterialPageRoute(
                                        builder: (_) => CourseDetailsScreen(
                                          courseId: courseProgress.courseId,
                                        ),
                                      ),
                                    );
                                  },
                                );
                              }
                              return const SizedBox.shrink();
                            },
                          );
                        }).toList(),
                        const SizedBox(height: AppSpacing.xl),
                      ],
                    );
                  }
                  return const SizedBox.shrink();
                },
                loading: () => const SizedBox.shrink(),
                error: (_, __) => const SizedBox.shrink(),
              ),

              // Featured courses
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  AppText(
                    'Featured Courses',
                    variant: TextVariant.h4,
                    fontWeight: FontWeight.bold,
                  ),
                  TextButton(
                    onPressed: () {
                      Navigator.of(context).push(
                        MaterialPageRoute(
                          builder: (_) => const CourseListScreen(),
                        ),
                      );
                    },
                    child: const Text('View All'),
                  ),
                ],
              ),
              const SizedBox(height: AppSpacing.md),

              featuredCoursesAsync.when(
                data: (courses) {
                  return Column(
                    children: courses.take(3).map((course) {
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
                    }).toList(),
                  );
                },
                loading: () => const AppLoadingIndicator(),
                error: (error, _) => Center(child: Text('Error: $error')),
              ),
            ],
          ),
        ),
      ),
      floatingActionButton: FloatingActionButton.extended(
        onPressed: () {
          Navigator.of(context).push(
            MaterialPageRoute(
              builder: (_) => const CourseListScreen(),
            ),
          );
        },
        icon: const Icon(Icons.explore),
        label: const Text('Explore Courses'),
      ),
    );
  }

  Widget _buildWelcomeSection(BuildContext context) {
    return Container(
      padding: const EdgeInsets.all(AppSpacing.lg),
      decoration: BoxDecoration(
        gradient: const LinearGradient(
          colors: [Color(0xFF2196F3), Color(0xFF1976D2)],
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const Row(
            children: [
              Icon(
                Icons.wb_sunny,
                color: Colors.white,
                size: 28,
              ),
              SizedBox(width: AppSpacing.sm),
              Text(
                'Welcome back!',
                style: TextStyle(
                  fontSize: 24,
                  fontWeight: FontWeight.bold,
                  color: Colors.white,
                ),
              ),
            ],
          ),
          const SizedBox(height: AppSpacing.sm),
          const Text(
            'Ready to continue your learning journey?',
            style: TextStyle(
              fontSize: 14,
              color: Colors.white70,
            ),
          ),
          const SizedBox(height: AppSpacing.lg),
          AppButton(
            label: 'Browse Courses',
            variant: ButtonVariant.secondary,
            onPressed: () {
              Navigator.of(context).push(
                MaterialPageRoute(
                  builder: (_) => const CourseListScreen(),
                ),
              );
            },
          ),
        ],
      ),
    );
  }

  Widget _buildStatsCards(
    AsyncValue progressAsync,
    AsyncValue certificatesAsync,
  ) {
    return Row(
      children: [
        Expanded(
          child: _buildStatCard(
            'Enrolled',
            progressAsync.when(
              data: (progress) => progress.length.toString(),
              loading: () => '...',
              error: (_, __) => '0',
            ),
            Icons.school,
            AppColors.primary,
          ),
        ),
        const SizedBox(width: AppSpacing.md),
        Expanded(
          child: _buildStatCard(
            'In Progress',
            progressAsync.when(
              data: (progress) => progress
                  .where((p) => p.status == ProgressStatus.inProgress)
                  .length
                  .toString(),
              loading: () => '...',
              error: (_, __) => '0',
            ),
            Icons.play_circle,
            AppColors.warning,
          ),
        ),
        const SizedBox(width: AppSpacing.md),
        Expanded(
          child: _buildStatCard(
            'Completed',
            progressAsync.when(
              data: (progress) => progress
                  .where((p) => p.status == ProgressStatus.completed)
                  .length
                  .toString(),
              loading: () => '...',
              error: (_, __) => '0',
            ),
            Icons.check_circle,
            AppColors.success,
          ),
        ),
      ],
    );
  }

  Widget _buildStatCard(String label, String value, IconData icon, Color color) {
    return Container(
      padding: const EdgeInsets.all(AppSpacing.md),
      decoration: BoxDecoration(
        color: AppColors.surface,
        borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
        border: Border.all(color: AppColors.border),
      ),
      child: Column(
        children: [
          Icon(icon, size: 32, color: color),
          const SizedBox(height: AppSpacing.sm),
          AppText(
            value,
            variant: TextVariant.h3,
            fontWeight: FontWeight.bold,
            color: color,
          ),
          AppText(
            label,
            variant: TextVariant.bodySmall,
            color: AppColors.textSecondary,
          ),
        ],
      ),
    );
  }
}
