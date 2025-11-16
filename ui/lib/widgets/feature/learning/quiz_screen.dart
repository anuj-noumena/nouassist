import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:ui/providers/course_provider.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/quiz_dto.dart';
import '../../../providers/quiz_provider.dart';
import '../../../providers/certificate_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_button.dart';
import '../../atomic/app_loading_indicator.dart';
import 'widgets/quiz_question_widget.dart';

class QuizScreen extends ConsumerStatefulWidget {
  final String quizId;

  const QuizScreen({super.key, required this.quizId});

  @override
  ConsumerState<QuizScreen> createState() => _QuizScreenState();
}

class _QuizScreenState extends ConsumerState<QuizScreen> {
  final Map<String, String> _answers = {};
  bool _isSubmitting = false;
  QuizAttemptDto? _attemptResult;
  int _currentQuestionIndex = 0;

  @override
  Widget build(BuildContext context) {
    final quizAsync = ref.watch(quizProvider(widget.quizId));

    return Scaffold(
      appBar: AppBar(
        title: const Text('Quiz'),
        actions: [
          if (_attemptResult == null)
            Padding(
              padding: const EdgeInsets.all(AppSpacing.md),
              child: Center(
                child: Text(
                  '${_currentQuestionIndex + 1}/${quizAsync.value?.questions.length ?? 0}',
                  style: const TextStyle(
                    fontSize: 16,
                    fontWeight: FontWeight.w600,
                  ),
                ),
              ),
            ),
        ],
      ),
      body: quizAsync.when(
        data: (quiz) => _attemptResult != null
            ? _buildResults(context, quiz, _attemptResult!)
            : _buildQuiz(context, quiz),
        loading: () => const AppLoadingIndicator(),
        error: (error, _) => Center(child: Text('Error: $error')),
      ),
    );
  }

  Widget _buildQuiz(BuildContext context, QuizDto quiz) {
    final question = quiz.questions[_currentQuestionIndex];

    return Column(
      children: [
        // Progress bar
        LinearProgressIndicator(
          value: (_currentQuestionIndex + 1) / quiz.questions.length,
          backgroundColor: AppColors.border,
          valueColor: const AlwaysStoppedAnimation<Color>(AppColors.primary),
        ),
        Expanded(
          child: SingleChildScrollView(
            padding: const EdgeInsets.all(AppSpacing.md),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                // Quiz info
                Container(
                  padding: const EdgeInsets.all(AppSpacing.md),
                  decoration: BoxDecoration(
                    color: AppColors.info.withOpacity(0.1),
                    borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
                    border: Border.all(color: AppColors.info),
                  ),
                  child: Row(
                    children: [
                      const Icon(Icons.info_outline, color: AppColors.info),
                      const SizedBox(width: AppSpacing.sm),
                      Expanded(
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            AppText(
                              quiz.title,
                              variant: TextVariant.bodyLarge,
                              fontWeight: FontWeight.w600,
                            ),
                            const SizedBox(height: AppSpacing.xs),
                            AppText(
                              'Passing score: ${quiz.passingScore}% • Time limit: ${quiz.timeLimit} min',
                              variant: TextVariant.bodySmall,
                              color: AppColors.info,
                            ),
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: AppSpacing.xl),
                // Question
                QuizQuestionWidget(
                  question: question,
                  selectedAnswer: _answers[question.id],
                  onAnswerSelected: (answer) {
                    setState(() {
                      _answers[question.id] = answer;
                    });
                  },
                ),
              ],
            ),
          ),
        ),
        // Navigation buttons
        _buildNavigationButtons(quiz),
      ],
    );
  }

  Widget _buildNavigationButtons(QuizDto quiz) {
    final isLastQuestion = _currentQuestionIndex == quiz.questions.length - 1;
    final canProceed = _answers.containsKey(quiz.questions[_currentQuestionIndex].id);

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
            if (_currentQuestionIndex > 0)
              Expanded(
                child: AppButton(
                  label: 'Previous',
                  variant: ButtonVariant.outlined,
                  onPressed: () {
                    setState(() {
                      _currentQuestionIndex--;
                    });
                  },
                ),
              ),
            if (_currentQuestionIndex > 0) const SizedBox(width: AppSpacing.md),
            Expanded(
              child: AppButton(
                label: isLastQuestion ? 'Submit Quiz' : 'Next',
                isLoading: _isSubmitting,
                onPressed: canProceed
                    ? () {
                        if (isLastQuestion) {
                          _submitQuiz(quiz);
                        } else {
                          setState(() {
                            _currentQuestionIndex++;
                          });
                        }
                      }
                    : null,
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildResults(
    BuildContext context,
    QuizDto quiz,
    QuizAttemptDto attempt,
  ) {
    final passed = attempt.passed;

    return SingleChildScrollView(
      padding: const EdgeInsets.all(AppSpacing.md),
      child: Column(
        children: [
          // Result card
          Container(
            padding: const EdgeInsets.all(AppSpacing.xl),
            decoration: BoxDecoration(
              gradient: LinearGradient(
                colors: passed
                    ? [AppColors.success, AppColors.success.withOpacity(0.8)]
                    : [AppColors.error, AppColors.error.withOpacity(0.8)],
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
              ),
              borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
            ),
            child: Column(
              children: [
                Icon(
                  passed ? Icons.celebration : Icons.error_outline,
                  size: 80,
                  color: Colors.white,
                ),
                const SizedBox(height: AppSpacing.lg),
                Text(
                  passed ? 'Congratulations!' : 'Keep Trying!',
                  style: const TextStyle(
                    fontSize: 28,
                    fontWeight: FontWeight.bold,
                    color: Colors.white,
                  ),
                ),
                const SizedBox(height: AppSpacing.sm),
                Text(
                  passed
                      ? 'You passed the quiz!'
                      : 'You didn\'t pass this time',
                  style: const TextStyle(
                    fontSize: 16,
                    color: Colors.white70,
                  ),
                ),
                const SizedBox(height: AppSpacing.xl),
                Container(
                  padding: const EdgeInsets.all(AppSpacing.lg),
                  decoration: BoxDecoration(
                    color: Colors.white,
                    borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
                  ),
                  child: Column(
                    children: [
                      Text(
                        '${attempt.percentage.toStringAsFixed(1)}%',
                        style: TextStyle(
                          fontSize: 48,
                          fontWeight: FontWeight.bold,
                          color: passed ? AppColors.success : AppColors.error,
                        ),
                      ),
                      Text(
                        'Your Score',
                        style: TextStyle(
                          fontSize: 16,
                          color: AppColors.textSecondary,
                        ),
                      ),
                      const SizedBox(height: AppSpacing.md),
                      Text(
                        '${attempt.score} / ${attempt.totalPoints} points',
                        style: const TextStyle(
                          fontSize: 14,
                          fontWeight: FontWeight.w600,
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ),
          ),
          const SizedBox(height: AppSpacing.xl),
          // Stats
          Row(
            children: [
              Expanded(
                child: _buildStatCard(
                  'Correct',
                  _calculateCorrectAnswers(quiz, attempt).toString(),
                  AppColors.success,
                ),
              ),
              const SizedBox(width: AppSpacing.md),
              Expanded(
                child: _buildStatCard(
                  'Wrong',
                  (quiz.questions.length - _calculateCorrectAnswers(quiz, attempt))
                      .toString(),
                  AppColors.error,
                ),
              ),
              const SizedBox(width: AppSpacing.md),
              Expanded(
                child: _buildStatCard(
                  'Time',
                  '${attempt.timeSpent} min',
                  AppColors.info,
                ),
              ),
            ],
          ),
          const SizedBox(height: AppSpacing.xl),
          // Review answers
          if (quiz.showCorrectAnswers) ...[
            AppText(
              'Review Answers',
              variant: TextVariant.h4,
              fontWeight: FontWeight.bold,
            ),
            const SizedBox(height: AppSpacing.md),
            ...quiz.questions.asMap().entries.map((entry) {
              final index = entry.key;
              final question = entry.value;
              final userAnswer = attempt.answers[question.id];
              final isCorrect = userAnswer == question.correctAnswer;

              return Card(
                margin: const EdgeInsets.only(bottom: AppSpacing.md),
                child: Padding(
                  padding: const EdgeInsets.all(AppSpacing.md),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Row(
                        children: [
                          Container(
                            padding: const EdgeInsets.all(AppSpacing.sm),
                            decoration: BoxDecoration(
                              color: isCorrect
                                  ? AppColors.success.withOpacity(0.1)
                                  : AppColors.error.withOpacity(0.1),
                              shape: BoxShape.circle,
                            ),
                            child: Icon(
                              isCorrect ? Icons.check : Icons.close,
                              color: isCorrect ? AppColors.success : AppColors.error,
                              size: 20,
                            ),
                          ),
                          const SizedBox(width: AppSpacing.sm),
                          Expanded(
                            child: AppText(
                              'Question ${index + 1}',
                              variant: TextVariant.bodyLarge,
                              fontWeight: FontWeight.w600,
                            ),
                          ),
                          Container(
                            padding: const EdgeInsets.symmetric(
                              horizontal: AppSpacing.sm,
                              vertical: AppSpacing.xs,
                            ),
                            decoration: BoxDecoration(
                              color: AppColors.primary.withOpacity(0.1),
                              borderRadius:
                                  BorderRadius.circular(AppSpacing.radiusSm),
                            ),
                            child: Text(
                              '${question.points} pts',
                              style: const TextStyle(
                                fontSize: 12,
                                fontWeight: FontWeight.w600,
                                color: AppColors.primary,
                              ),
                            ),
                          ),
                        ],
                      ),
                      const SizedBox(height: AppSpacing.md),
                      AppText(
                        question.question,
                        variant: TextVariant.bodyMedium,
                      ),
                      const SizedBox(height: AppSpacing.md),
                      if (userAnswer != null) ...[
                        Container(
                          padding: const EdgeInsets.all(AppSpacing.sm),
                          decoration: BoxDecoration(
                            color: isCorrect
                                ? AppColors.success.withOpacity(0.1)
                                : AppColors.error.withOpacity(0.1),
                            borderRadius:
                                BorderRadius.circular(AppSpacing.radiusSm),
                            border: Border.all(
                              color: isCorrect ? AppColors.success : AppColors.error,
                            ),
                          ),
                          child: Row(
                            children: [
                              Icon(
                                isCorrect ? Icons.check_circle : Icons.cancel,
                                size: 16,
                                color: isCorrect ? AppColors.success : AppColors.error,
                              ),
                              const SizedBox(width: AppSpacing.sm),
                              Expanded(
                                child: Text(
                                  'Your answer: $userAnswer',
                                  style: TextStyle(
                                    fontSize: 14,
                                    fontWeight: FontWeight.w600,
                                    color: isCorrect
                                        ? AppColors.success
                                        : AppColors.error,
                                  ),
                                ),
                              ),
                            ],
                          ),
                        ),
                        if (!isCorrect) ...[
                          const SizedBox(height: AppSpacing.sm),
                          Container(
                            padding: const EdgeInsets.all(AppSpacing.sm),
                            decoration: BoxDecoration(
                              color: AppColors.success.withOpacity(0.1),
                              borderRadius:
                                  BorderRadius.circular(AppSpacing.radiusSm),
                              border: Border.all(color: AppColors.success),
                            ),
                            child: Row(
                              children: [
                                const Icon(
                                  Icons.check_circle,
                                  size: 16,
                                  color: AppColors.success,
                                ),
                                const SizedBox(width: AppSpacing.sm),
                                Expanded(
                                  child: Text(
                                    'Correct answer: ${question.correctAnswer}',
                                    style: const TextStyle(
                                      fontSize: 14,
                                      fontWeight: FontWeight.w600,
                                      color: AppColors.success,
                                    ),
                                  ),
                                ),
                              ],
                            ),
                          ),
                        ],
                      ],
                      if (question.explanation != null) ...[
                        const SizedBox(height: AppSpacing.sm),
                        Container(
                          padding: const EdgeInsets.all(AppSpacing.sm),
                          decoration: BoxDecoration(
                            color: AppColors.info.withOpacity(0.1),
                            borderRadius:
                                BorderRadius.circular(AppSpacing.radiusSm),
                          ),
                          child: Row(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              const Icon(
                                Icons.info_outline,
                                size: 16,
                                color: AppColors.info,
                              ),
                              const SizedBox(width: AppSpacing.sm),
                              Expanded(
                                child: Text(
                                  question.explanation!,
                                  style: const TextStyle(
                                    fontSize: 12,
                                    color: AppColors.info,
                                  ),
                                ),
                              ),
                            ],
                          ),
                        ),
                      ],
                    ],
                  ),
                ),
              );
            }).toList(),
          ],
          const SizedBox(height: AppSpacing.xl),
          // Action buttons
          if (!passed && attempt.attemptNumber < quiz.maxAttempts) ...[
            AppButton(
              label: 'Retake Quiz (${attempt.attemptNumber}/${quiz.maxAttempts} attempts)',
              onPressed: () {
                setState(() {
                  _answers.clear();
                  _attemptResult = null;
                  _currentQuestionIndex = 0;
                });
              },
            ),
            const SizedBox(height: AppSpacing.md),
          ],
          AppButton(
            label: 'Back to Course',
            variant: ButtonVariant.outlined,
            onPressed: () {
              Navigator.of(context).pop();
            },
          ),
        ],
      ),
    );
  }

  Widget _buildStatCard(String label, String value, Color color) {
    return Container(
      padding: const EdgeInsets.all(AppSpacing.md),
      decoration: BoxDecoration(
        color: AppColors.surface,
        borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
        border: Border.all(color: AppColors.border),
      ),
      child: Column(
        children: [
          Text(
            value,
            style: TextStyle(
              fontSize: 24,
              fontWeight: FontWeight.bold,
              color: color,
            ),
          ),
          const SizedBox(height: AppSpacing.xs),
          Text(
            label,
            style: const TextStyle(
              fontSize: 12,
              color: AppColors.textSecondary,
            ),
          ),
        ],
      ),
    );
  }

  int _calculateCorrectAnswers(QuizDto quiz, QuizAttemptDto attempt) {
    int correct = 0;
    for (var question in quiz.questions) {
      if (attempt.answers[question.id] == question.correctAnswer) {
        correct++;
      }
    }
    return correct;
  }

  Future<void> _submitQuiz(QuizDto quiz) async {
    // Check if all questions are answered
    if (_answers.length < quiz.questions.length) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Please answer all questions before submitting'),
          backgroundColor: AppColors.warning,
        ),
      );
      return;
    }

    setState(() {
      _isSubmitting = true;
    });

    try {
      final attempt = await ref.read(
        submitQuizProvider(
          userId: 'user-1',
          quizId: widget.quizId,
          answers: _answers,
        ).future,
      );

      setState(() {
        _attemptResult = attempt;
        _isSubmitting = false;
      });

      // If passed and course is certified, issue certificate
      if (attempt.passed) {
        final courseAsync = await ref.read(courseProvider(quiz.courseId).future);
        if (courseAsync.isCertified) {
          try {
            await ref.read(
              issueCertificateProvider(
                userId: 'user-1',
                userName: 'John Doe',
                courseId: quiz.courseId,
                courseTitle: courseAsync.title,
                instructorName: courseAsync.instructorName,
                finalScore: attempt.percentage,
              ).future,
            );
          } catch (e) {
            // Certificate already issued or error
          }
        }
      }
    } catch (e) {
      setState(() {
        _isSubmitting = false;
      });

      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to submit quiz: $e'),
            backgroundColor: AppColors.error,
          ),
        );
      }
    }
  }
}
