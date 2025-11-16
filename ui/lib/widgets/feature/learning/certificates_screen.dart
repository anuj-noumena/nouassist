import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../core/utils/extensions.dart';
import '../../../data/models/certificate_dto.dart';
import '../../../providers/certificate_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_button.dart';
import '../../atomic/app_loading_indicator.dart';
import '../../atomic/app_error_widget.dart';

class CertificatesScreen extends ConsumerWidget {
  const CertificatesScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final certificatesAsync = ref.watch(userCertificatesProvider('user-1'));

    return Scaffold(
      appBar: AppBar(
        title: const Text('My Certificates'),
      ),
      body: certificatesAsync.when(
        data: (certificates) {
          if (certificates.isEmpty) {
            return Center(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Icon(
                    Icons.workspace_premium,
                    size: 80,
                    color: AppColors.textDisabled,
                  ),
                  const SizedBox(height: AppSpacing.lg),
                  AppText(
                    'No certificates yet',
                    variant: TextVariant.h5,
                    color: AppColors.textSecondary,
                  ),
                  const SizedBox(height: AppSpacing.sm),
                  AppText(
                    'Complete certified courses to earn certificates',
                    variant: TextVariant.bodyMedium,
                    color: AppColors.textSecondary,
                  ),
                ],
              ),
            );
          }

          return ListView.builder(
            padding: const EdgeInsets.all(AppSpacing.md),
            itemCount: certificates.length,
            itemBuilder: (context, index) {
              final certificate = certificates[index];
              return _buildCertificateCard(context, certificate);
            },
          );
        },
        loading: () => const AppLoadingIndicator(),
        error: (error, _) => AppErrorWidget(error: error.toString()),
      ),
    );
  }

  Widget _buildCertificateCard(BuildContext context, CertificateDto certificate) {
    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.md),
      child: InkWell(
        onTap: () => _showCertificateDetails(context, certificate),
        borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
        child: Container(
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
            gradient: LinearGradient(
              colors: [
                AppColors.primary.withOpacity(0.1),
                AppColors.primary.withOpacity(0.05),
              ],
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
          ),
          child: Padding(
            padding: const EdgeInsets.all(AppSpacing.lg),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  children: [
                    Container(
                      padding: const EdgeInsets.all(AppSpacing.md),
                      decoration: BoxDecoration(
                        color: AppColors.warning.withOpacity(0.2),
                        shape: BoxShape.circle,
                      ),
                      child: const Icon(
                        Icons.workspace_premium,
                        size: 32,
                        color: AppColors.warning,
                      ),
                    ),
                    const SizedBox(width: AppSpacing.md),
                    Expanded(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          AppText(
                            certificate.courseTitle,
                            variant: TextVariant.bodyLarge,
                            fontWeight: FontWeight.bold,
                          ),
                          const SizedBox(height: AppSpacing.xs),
                          AppText(
                            'By ${certificate.instructorName}',
                            variant: TextVariant.bodySmall,
                            color: AppColors.textSecondary,
                          ),
                        ],
                      ),
                    ),
                    if (certificate.isVerified)
                      Container(
                        padding: const EdgeInsets.all(AppSpacing.xs),
                        decoration: BoxDecoration(
                          color: AppColors.success.withOpacity(0.1),
                          shape: BoxShape.circle,
                        ),
                        child: const Icon(
                          Icons.verified,
                          size: 20,
                          color: AppColors.success,
                        ),
                      ),
                  ],
                ),
                const SizedBox(height: AppSpacing.lg),
                Divider(color: AppColors.border),
                const SizedBox(height: AppSpacing.md),
                Row(
                  children: [
                    _buildInfoItem(
                      Icons.calendar_today,
                      'Issued',
                      certificate.issuedDate.toShortDateString(),
                    ),
                    const SizedBox(width: AppSpacing.xl),
                    _buildInfoItem(
                      Icons.star,
                      'Score',
                      '${certificate.finalScore.toStringAsFixed(1)}%',
                    ),
                  ],
                ),
                const SizedBox(height: AppSpacing.sm),
                AppText(
                  'Certificate #${certificate.certificateNumber}',
                  variant: TextVariant.bodySmall,
                  color: AppColors.textSecondary,
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildInfoItem(IconData icon, String label, String value) {
    return Row(
      mainAxisSize: MainAxisSize.min,
      children: [
        Icon(icon, size: 16, color: AppColors.textSecondary),
        const SizedBox(width: AppSpacing.xs),
        Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            AppText(
              label,
              variant: TextVariant.bodySmall,
              color: AppColors.textSecondary,
            ),
            AppText(
              value,
              variant: TextVariant.bodyMedium,
              fontWeight: FontWeight.w600,
            ),
          ],
        ),
      ],
    );
  }

  void _showCertificateDetails(BuildContext context, CertificateDto certificate) {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      shape: const RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(
          top: Radius.circular(AppSpacing.radiusMd),
        ),
      ),
      builder: (context) => DraggableScrollableSheet(
        initialChildSize: 0.9,
        minChildSize: 0.5,
        maxChildSize: 0.95,
        expand: false,
        builder: (context, scrollController) => SingleChildScrollView(
          controller: scrollController,
          padding: const EdgeInsets.all(AppSpacing.xl),
          child: Column(
            children: [
              // Certificate design
              Container(
                padding: const EdgeInsets.all(AppSpacing.xl),
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
                  border: Border.all(color: AppColors.warning, width: 8),
                  boxShadow: [
                    BoxShadow(
                      color: Colors.black.withOpacity(0.1),
                      blurRadius: 20,
                      offset: const Offset(0, 10),
                    ),
                  ],
                ),
                child: Column(
                  children: [
                    const Icon(
                      Icons.workspace_premium,
                      size: 80,
                      color: AppColors.warning,
                    ),
                    const SizedBox(height: AppSpacing.lg),
                    const Text(
                      'CERTIFICATE OF COMPLETION',
                      style: TextStyle(
                        fontSize: 18,
                        fontWeight: FontWeight.bold,
                        letterSpacing: 2,
                        color: AppColors.primary,
                      ),
                    ),
                    const SizedBox(height: AppSpacing.xl),
                    const Text(
                      'This certifies that',
                      style: TextStyle(
                        fontSize: 14,
                        color: AppColors.textSecondary,
                      ),
                    ),
                    const SizedBox(height: AppSpacing.sm),
                    Text(
                      certificate.userName,
                      style: const TextStyle(
                        fontSize: 32,
                        fontWeight: FontWeight.bold,
                        color: AppColors.textPrimary,
                      ),
                    ),
                    const SizedBox(height: AppSpacing.xl),
                    const Text(
                      'has successfully completed',
                      style: TextStyle(
                        fontSize: 14,
                        color: AppColors.textSecondary,
                      ),
                    ),
                    const SizedBox(height: AppSpacing.sm),
                    Text(
                      certificate.courseTitle,
                      textAlign: TextAlign.center,
                      style: const TextStyle(
                        fontSize: 24,
                        fontWeight: FontWeight.bold,
                        color: AppColors.primary,
                      ),
                    ),
                    const SizedBox(height: AppSpacing.xl),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Column(
                          children: [
                            Text(
                              certificate.issuedDate.toShortDateString(),
                              style: const TextStyle(
                                fontSize: 16,
                                fontWeight: FontWeight.w600,
                              ),
                            ),
                            const SizedBox(height: AppSpacing.xs),
                            const Text(
                              'Date',
                              style: TextStyle(
                                fontSize: 12,
                                color: AppColors.textSecondary,
                              ),
                            ),
                          ],
                        ),
                        const SizedBox(width: AppSpacing.xxl),
                        Column(
                          children: [
                            Text(
                              '${certificate.finalScore.toStringAsFixed(1)}%',
                              style: const TextStyle(
                                fontSize: 16,
                                fontWeight: FontWeight.w600,
                              ),
                            ),
                            const SizedBox(height: AppSpacing.xs),
                            const Text(
                              'Final Score',
                              style: TextStyle(
                                fontSize: 12,
                                color: AppColors.textSecondary,
                              ),
                            ),
                          ],
                        ),
                      ],
                    ),
                    const SizedBox(height: AppSpacing.xl),
                    Text(
                      certificate.certificateNumber,
                      style: const TextStyle(
                        fontSize: 10,
                        color: AppColors.textSecondary,
                        letterSpacing: 1,
                      ),
                    ),
                  ],
                ),
              ),
              const SizedBox(height: AppSpacing.xl),
              // Actions
              AppButton(
                label: 'Download Certificate',
                icon: Icons.download,
                onPressed: () {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(
                      content: Text('Certificate download started'),
                      backgroundColor: AppColors.success,
                    ),
                  );
                },
              ),
              const SizedBox(height: AppSpacing.md),
              if (certificate.verificationUrl != null)
                AppButton(
                  label: 'Verify Certificate',
                  variant: ButtonVariant.outlined,
                  icon: Icons.verified,
                  onPressed: () {
                    // Open verification URL
                  },
                ),
              const SizedBox(height: AppSpacing.md),
              AppButton(
                label: 'Share',
                variant: ButtonVariant.outlined,
                icon: Icons.share,
                onPressed: () {
                  // Share certificate
                },
              ),
            ],
          ),
        ),
      ),
    );
  }
}
