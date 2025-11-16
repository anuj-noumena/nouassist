import 'package:intl/intl.dart';

/// DateTime extensions for formatting
extension DateTimeExtensions on DateTime {
  /// Format as "Jan 15, 2024"
  String toShortDateString() {
    return DateFormat('MMM d, y').format(this);
  }

  /// Format as "January 15, 2024"
  String toLongDateString() {
    return DateFormat('MMMM d, y').format(this);
  }

  /// Format as "01/15/2024"
  String toNumericDateString() {
    return DateFormat('MM/dd/y').format(this);
  }

  /// Format as "3:30 PM"
  String toTimeString() {
    return DateFormat('h:mm a').format(this);
  }

  /// Format as "Jan 15, 2024 3:30 PM"
  String toDateTimeString() {
    return DateFormat('MMM d, y h:mm a').format(this);
  }

  /// Check if date is today
  bool get isToday {
    final now = DateTime.now();
    return year == now.year && month == now.month && day == now.day;
  }

  /// Check if date is tomorrow
  bool get isTomorrow {
    final tomorrow = DateTime.now().add(const Duration(days: 1));
    return year == tomorrow.year &&
        month == tomorrow.month &&
        day == tomorrow.day;
  }

  /// Check if date is in the past
  bool get isPast {
    return isBefore(DateTime.now());
  }

  /// Get relative time string (e.g., "2 hours ago", "in 3 days")
  String toRelativeTimeString() {
    final now = DateTime.now();
    final difference = this.difference(now);

    if (difference.isNegative) {
      final absDiff = difference.abs();
      if (absDiff.inDays > 365) {
        final years = (absDiff.inDays / 365).floor();
        return '$years ${years == 1 ? 'year' : 'years'} ago';
      } else if (absDiff.inDays > 30) {
        final months = (absDiff.inDays / 30).floor();
        return '$months ${months == 1 ? 'month' : 'months'} ago';
      } else if (absDiff.inDays > 0) {
        return '${absDiff.inDays} ${absDiff.inDays == 1 ? 'day' : 'days'} ago';
      } else if (absDiff.inHours > 0) {
        return '${absDiff.inHours} ${absDiff.inHours == 1 ? 'hour' : 'hours'} ago';
      } else if (absDiff.inMinutes > 0) {
        return '${absDiff.inMinutes} ${absDiff.inMinutes == 1 ? 'minute' : 'minutes'} ago';
      } else {
        return 'just now';
      }
    } else {
      if (difference.inDays > 365) {
        final years = (difference.inDays / 365).floor();
        return 'in $years ${years == 1 ? 'year' : 'years'}';
      } else if (difference.inDays > 30) {
        final months = (difference.inDays / 30).floor();
        return 'in $months ${months == 1 ? 'month' : 'months'}';
      } else if (difference.inDays > 0) {
        return 'in ${difference.inDays} ${difference.inDays == 1 ? 'day' : 'days'}';
      } else if (difference.inHours > 0) {
        return 'in ${difference.inHours} ${difference.inHours == 1 ? 'hour' : 'hours'}';
      } else if (difference.inMinutes > 0) {
        return 'in ${difference.inMinutes} ${difference.inMinutes == 1 ? 'minute' : 'minutes'}';
      } else {
        return 'now';
      }
    }
  }
}

/// String extensions
extension StringExtensions on String {
  /// Capitalize first letter
  String capitalize() {
    if (isEmpty) return this;
    return '${this[0].toUpperCase()}${substring(1)}';
  }

  /// Capitalize first letter of each word
  String capitalizeWords() {
    if (isEmpty) return this;
    return split(' ').map((word) => word.capitalize()).join(' ');
  }

  /// Truncate string with ellipsis
  String truncate(int maxLength, {String ellipsis = '...'}) {
    if (length <= maxLength) return this;
    return '${substring(0, maxLength - ellipsis.length)}$ellipsis';
  }
}

/// List extensions
extension ListExtensions<T> on List<T> {
  /// Get element at index or null if out of bounds
  T? getOrNull(int index) {
    if (index < 0 || index >= length) return null;
    return this[index];
  }
}
