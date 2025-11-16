import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/lesson_dto.dart';
import '../../../providers/lesson_provider.dart';
import '../../../providers/progress_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_button.dart';
import '../../atomic/app_loading_indicator.dart';
import 'quiz_screen.dart';

class LessonViewerScreen extends ConsumerStatefulWidget {
  final String lessonId;
  final String courseId;

  const LessonViewerScreen({
    super.key,
    required this.lessonId,
    required this.courseId,
  });

  @override
  ConsumerState<LessonViewerScreen> createState() =>
      _LessonViewerScreenState();
}

class _LessonViewerScreenState extends ConsumerState<LessonViewerScreen> {
  bool _isCompleted = false;

  @override
  Widget build(BuildContext context) {
    final lessonAsync = ref.watch(lessonProvider(widget.lessonId));

    return Scaffold(
      appBar: AppBar(
        title: const Text('Lesson'),
      ),
      body: lessonAsync.when(
        data: (lesson) => _buildLessonContent(context, lesson),
        loading: () => const AppLoadingIndicator(),
        error: (error, _) => Center(child: Text('Error: $error')),
      ),
    );
  }

  Widget _buildLessonContent(BuildContext context, LessonDto lesson) {
    return Column(
      children: [
        Expanded(
          child: SingleChildScrollView(
            padding: const EdgeInsets.all(AppSpacing.md),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                // Lesson title
                AppText(
                  lesson.title,
                  variant: TextVariant.h3,
                  fontWeight: FontWeight.bold,
                ),
                const SizedBox(height: AppSpacing.md),
                // Lesson info
                Row(
                  children: [
                    _buildLessonTypeChip(lesson.lessonType),
                    const SizedBox(width: AppSpacing.sm),
                    Icon(
                      Icons.access_time,
                      size: 16,
                      color: AppColors.textSecondary,
                    ),
                    const SizedBox(width: AppSpacing.xs),
                    AppText(
                      '${lesson.duration} min',
                      variant: TextVariant.bodySmall,
                      color: AppColors.textSecondary,
                    ),
                  ],
                ),
                const SizedBox(height: AppSpacing.xl),
                // Video player placeholder
                if (lesson.lessonType == LessonType.video) ...[
                  Container(
                    height: 200,
                    decoration: BoxDecoration(
                      color: Colors.black,
                      borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
                    ),
                    child: Center(
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Icon(
                            Icons.play_circle_outline,
                            size: 64,
                            color: Colors.white.withOpacity(0.7),
                          ),
                          const SizedBox(height: AppSpacing.sm),
                          Text(
                            'Video Player',
                            style: TextStyle(
                              color: Colors.white.withOpacity(0.7),
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
                  const SizedBox(height: AppSpacing.xl),
                ],
                // Description
                AppText(
                  'Description',
                  variant: TextVariant.h5,
                  fontWeight: FontWeight.w600,
                ),
                const SizedBox(height: AppSpacing.sm),
                AppText(
                  lesson.description,
                  variant: TextVariant.bodyMedium,
                  color: AppColors.textSecondary,
                ),
                const SizedBox(height: AppSpacing.xl),
                // Content
                if (lesson.content != null) ...[
                  AppText(
                    'Content',
                    variant: TextVariant.h5,
                    fontWeight: FontWeight.w600,
                  ),
                  const SizedBox(height: AppSpacing.sm),
                  AppText(
                    lesson.content!,
                    variant: TextVariant.bodyMedium,
                  ),
                  const SizedBox(height: AppSpacing.xl),
                ],
                // Attachments
                if (lesson.attachments.isNotEmpty) ...[
                  AppText(
                    'Attachments',
                    variant: TextVariant.h5,
                    fontWeight: FontWeight.w600,
                  ),
                  const SizedBox(height: AppSpacing.sm),
                  ...lesson.attachments.map((attachment) {
                    return ListTile(
                      leading: const Icon(Icons.attach_file),
                      title: Text(attachment),
                      trailing: const Icon(Icons.download),
                      onTap: () {
                        // Download attachment
                      },
                    );
                  }).toList(),
                ],
              ],
            ),
          ),
        ),
        // Bottom action buttons
        _buildBottomActions(context, lesson),
      ],
    );
  }

  Widget _buildLessonTypeChip(LessonType type) {
    IconData icon;
    Color color;

    switch (type) {
      case LessonType.video:
        icon = Icons.play_circle_outline;
        color = AppColors.primary;
        break;
      case LessonType.article:
        icon = Icons.article_outlined;
        color = AppColors.info;
        break;
      case LessonType.quiz:
        icon = Icons.quiz_outlined;
        color = AppColors.warning;
        break;
      case LessonType.assignment:
        icon = Icons.assignment_outlined;
        color = AppColors.error;
        break;
      case LessonType.exercise:
        icon = Icons.fitness_center;
        color = AppColors.success;
        break;
      case LessonType.project:
        icon = Icons.work_outline;
        color = const Color(0xFF9C27B0);
        break;
    }

    return Container(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.sm,
        vertical: AppSpacing.xs,
      ),
      decoration: BoxDecoration(
        color: color.withOpacity(0.1),
        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(icon, size: 16, color: color),
          const SizedBox(width: AppSpacing.xs),
          Text(
            type.displayName,
            style: TextStyle(
              fontSize: 12,
              fontWeight: FontWeight.w600,
              color: color,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildBottomActions(BuildContext context, LessonDto lesson) {
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
            if (lesson.lessonType == LessonType.quiz && lesson.quizId != null)
              Expanded(
                child: AppButton(
                  label: 'Take Quiz',
                  onPressed: () {
                    Navigator.of(context).push(
                      MaterialPageRoute(
                        builder: (_) => QuizScreen(quizId: lesson.quizId!),
                      ),
                    );
                  },
                ),
              )
            else
              Expanded(
                child: AppButton(
                  label: _isCompleted
                      ? 'Completed ✓'
                      : 'Mark as Complete',
                  variant: _isCompleted
                      ? ButtonVariant.outlined
                      : ButtonVariant.primary,
                  onPressed: _isCompleted ? null : () => _markComplete(lesson),
                ),
              ),
          ],
        ),
      ),
    );
  }

  Future<void> _markComplete(LessonDto lesson) async {
    try {
      await ref.read(
        markLessonCompleteProvider(
          'user-1',
          lesson.id,
          widget.courseId,
          lesson.duration,
        ).future,
      );

      setState(() {
        _isCompleted = true;
      });

      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
            content: Text('Lesson marked as complete!'),
            backgroundColor: AppColors.success,
          ),
        );
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to mark lesson as complete: $e'),
            backgroundColor: AppColors.error,
          ),
        );
      }
    }
  }
}
