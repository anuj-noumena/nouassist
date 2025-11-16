import 'package:flutter/material.dart';
import '../../../../core/theme/app_colors.dart';
import '../../../../core/theme/app_spacing.dart';
import '../../../../core/utils/extensions.dart';
import '../../../atomic/app_text.dart';
import '../../../atomic/app_button.dart';

class TaskDiscussionWidget extends StatefulWidget {
  final String taskId;

  const TaskDiscussionWidget({super.key, required this.taskId});

  @override
  State<TaskDiscussionWidget> createState() => _TaskDiscussionWidgetState();
}

class _TaskDiscussionWidgetState extends State<TaskDiscussionWidget> {
  final List<Comment> _comments = [
    Comment(
      id: '1',
      authorName: 'John Doe',
      content: 'Started working on this. Will have an initial implementation ready by EOD.',
      creationTime: DateTime.now().subtract(const Duration(hours: 3)),
    ),
    Comment(
      id: '2',
      authorName: 'Jane Smith',
      content: 'Great! Make sure to include unit tests.',
      creationTime: DateTime.now().subtract(const Duration(hours: 2)),
    ),
    Comment(
      id: '3',
      authorName: 'John Doe',
      content: 'Will do. Also planning to add integration tests.',
      creationTime: DateTime.now().subtract(const Duration(hours: 1)),
    ),
  ];

  final _commentController = TextEditingController();

  @override
  void dispose() {
    _commentController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        AppText(
          'Discussion',
          variant: TextVariant.h5,
          fontWeight: FontWeight.w600,
        ),
        const SizedBox(height: AppSpacing.md),
        if (_comments.isEmpty)
          Container(
            padding: const EdgeInsets.all(AppSpacing.lg),
            decoration: BoxDecoration(
              color: AppColors.surfaceVariant,
              borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
            ),
            child: Center(
              child: AppText(
                'No comments yet',
                variant: TextVariant.bodyMedium,
                color: AppColors.textSecondary,
              ),
            ),
          )
        else
          ..._comments.map((comment) => _buildComment(comment)).toList(),
        const SizedBox(height: AppSpacing.md),
        _buildCommentInput(),
      ],
    );
  }

  Widget _buildComment(Comment comment) {
    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.md),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.md),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                CircleAvatar(
                  radius: 16,
                  backgroundColor: AppColors.primary,
                  child: Text(
                    comment.authorName[0].toUpperCase(),
                    style: const TextStyle(
                      fontSize: 12,
                      color: Colors.white,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                ),
                const SizedBox(width: AppSpacing.sm),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      AppText(
                        comment.authorName,
                        variant: TextVariant.bodyMedium,
                        fontWeight: FontWeight.w600,
                      ),
                      AppText(
                        comment.creationTime.toRelativeTimeString(),
                        variant: TextVariant.bodySmall,
                        color: AppColors.textSecondary,
                      ),
                    ],
                  ),
                ),
                PopupMenuButton<String>(
                  itemBuilder: (context) => [
                    const PopupMenuItem(
                      value: 'edit',
                      child: Text('Edit'),
                    ),
                    const PopupMenuItem(
                      value: 'delete',
                      child: Text('Delete'),
                    ),
                  ],
                  onSelected: (value) {
                    if (value == 'delete') {
                      setState(() {
                        _comments.remove(comment);
                      });
                    }
                  },
                ),
              ],
            ),
            const SizedBox(height: AppSpacing.sm),
            AppText(
              comment.content,
              variant: TextVariant.bodyMedium,
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildCommentInput() {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.end,
      children: [
        Expanded(
          child: TextField(
            controller: _commentController,
            decoration: const InputDecoration(
              hintText: 'Add a comment...',
              border: OutlineInputBorder(),
            ),
            maxLines: 3,
          ),
        ),
        const SizedBox(width: AppSpacing.sm),
        AppButton(
          label: 'Post',
          isFullWidth: false,
          onPressed: _addComment,
        ),
      ],
    );
  }

  void _addComment() {
    if (_commentController.text.isNotEmpty) {
      setState(() {
        _comments.add(
          Comment(
            id: DateTime.now().millisecondsSinceEpoch.toString(),
            authorName: 'Current User',
            content: _commentController.text,
            creationTime: DateTime.now(),
          ),
        );
        _commentController.clear();
      });
    }
  }
}

class Comment {
  final String id;
  final String authorName;
  final String content;
  final DateTime creationTime;

  Comment({
    required this.id,
    required this.authorName,
    required this.content,
    required this.creationTime,
  });
}
