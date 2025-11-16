import 'package:flutter/material.dart';

/// Centralized color definitions for the application
class AppColors {
  AppColors._();

  // Primary colors
  static const primary = Color(0xFF2196F3);
  static const primaryDark = Color(0xFF1976D2);
  static const primaryLight = Color(0xFF64B5F6);
  
  // Secondary colors
  static const secondary = Color(0xFFFF9800);
  static const secondaryDark = Color(0xFFF57C00);
  static const secondaryLight = Color(0xFFFFB74D);
  
  // Neutral colors
  static const background = Color(0xFFF5F5F5);
  static const surface = Colors.white;
  static const surfaceVariant = Color(0xFFFAFAFA);
  
  // Status colors
  static const error = Color(0xFFD32F2F);
  static const success = Color(0xFF388E3C);
  static const warning = Color(0xFFF57C00);
  static const info = Color(0xFF1976D2);
  
  // Text colors
  static const textPrimary = Color(0xFF212121);
  static const textSecondary = Color(0xFF757575);
  static const textDisabled = Color(0xFFBDBDBD);
  static const textHint = Color(0xFF9E9E9E);
  static const textOnPrimary = Colors.white;
  
  // Border colors
  static const border = Color(0xFFE0E0E0);
  static const borderDark = Color(0xFFBDBDBD);
  static const divider = Color(0xFFEEEEEE);
  
  // Priority colors
  static const priorityLowest = Color(0xFF90CAF9);
  static const priorityLow = Color(0xFF64B5F6);
  static const priorityMedium = Color(0xFFFF9800);
  static const priorityHigh = Color(0xFFFF5722);
  static const priorityHighest = Color(0xFFD32F2F);
  
  // Status colors
  static const statusTodo = Color(0xFF9E9E9E);
  static const statusInProgress = Color(0xFF2196F3);
  static const statusInReview = Color(0xFFFF9800);
  static const statusDone = Color(0xFF4CAF50);
  static const statusBlocked = Color(0xFFD32F2F);
  
  // Overlay colors
  static const overlay = Color(0x80000000);
  static const overlayLight = Color(0x40000000);
}
