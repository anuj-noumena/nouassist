import 'package:flutter/material.dart';

enum TextVariant {
  h1,
  h2,
  h3,
  h4,
  h5,
  h6,
  bodyLarge,
  bodyMedium,
  bodySmall,
  caption,
  label,      // Added standard label
  labelSmall, 
}

class AppText extends StatelessWidget {
  final String text;
  final TextVariant variant;
  final Color? color;
  final FontWeight? fontWeight;
  final TextAlign? textAlign;
  final int? maxLines;
  final TextOverflow? overflow;
  final TextDecoration? decoration;

  const AppText(
    this.text, {
    super.key,
    this.variant = TextVariant.bodyMedium,
    this.color,
    this.fontWeight,
    this.textAlign,
    this.maxLines,
    this.overflow,
    this.decoration,
  });

  @override
  Widget build(BuildContext context) {
    return Text(
      text,
      style: _getTextStyle().copyWith(
        color: color,
        fontWeight: fontWeight,
        decoration: decoration,
      ),
      textAlign: textAlign,
      maxLines: maxLines,
      overflow: overflow,
    );
  }

  TextStyle _getTextStyle() {
    switch (variant) {
      case TextVariant.h1:
        return const TextStyle(fontSize: 32, fontWeight: FontWeight.bold);
      case TextVariant.h2:
        return const TextStyle(fontSize: 28, fontWeight: FontWeight.bold);
      case TextVariant.h3:
        return const TextStyle(fontSize: 24, fontWeight: FontWeight.bold);
      case TextVariant.h4:
        return const TextStyle(fontSize: 20, fontWeight: FontWeight.w600);
      case TextVariant.h5:
        return const TextStyle(fontSize: 18, fontWeight: FontWeight.w600);
      case TextVariant.h6:
        return const TextStyle(fontSize: 16, fontWeight: FontWeight.w600);
      
      // Body styles (Regular weight, meant for long text)
      case TextVariant.bodyLarge:
        return const TextStyle(fontSize: 16);
      case TextVariant.bodyMedium:
        return const TextStyle(fontSize: 14);
      case TextVariant.bodySmall:
        return const TextStyle(fontSize: 12);
        
      // Utility styles
      case TextVariant.caption:
        return const TextStyle(fontSize: 10, letterSpacing: 0.3);
        
      // Label styles (Medium weight, meant for UI elements/buttons)
      case TextVariant.label:
        return const TextStyle(fontSize: 12, fontWeight: FontWeight.w500);
      case TextVariant.labelSmall:
        return const TextStyle(fontSize: 11, fontWeight: FontWeight.w500, letterSpacing: 0.5);
    }
  }
}