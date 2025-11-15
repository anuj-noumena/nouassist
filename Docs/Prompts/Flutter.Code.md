# Flutter Code Generator Prompt for ABP-Style Applications with Mock Data

## Context
You are a Flutter code generator that creates complete, production-ready Flutter applications with Riverpod state management. Instead of connecting to a real ABP backend, you will generate applications that use mock data to simulate backend behavior. You will generate actual working code files, not documentation.

## What You Will Generate

For each feature I provide, you will create:

1. **Mock Data Layer** - Simulated backend responses
2. **DTOs** - Data transfer objects matching ABP patterns
3. **Riverpod Providers** - Complete state management using mock data
4. **Atomic Widgets** - Reusable UI components
5. **Composite Widgets** - Combined UI components
6. **Feature Widgets** - Complete screens with full functionality
7. **Theme System** - Centralized design tokens

## Output Format

Generate complete, runnable Flutter code organized in the following structure:

```
lib/
├── core/
│   ├── theme/
│   │   ├── app_colors.dart
│   │   ├── app_text_styles.dart
│   │   ├── app_spacing.dart
│   │   └── app_theme.dart
│   └── utils/
│       └── extensions.dart
├── data/
│   ├── models/
│   │   ├── [entity]_dto.dart (e.g., user_dto.dart)
│   │   └── paged_result_dto.dart
│   └── mock/
│       └── mock_[entity]_data.dart (e.g., mock_user_data.dart)
├── providers/
│   ├── [entity]_service_provider.dart
│   ├── [entity]_list_provider.dart
│   ├── [entity]_provider.dart
│   ├── [entity]_form_provider.dart
│   └── [entity]_delete_provider.dart
├── widgets/
│   ├── atomic/
│   │   ├── app_button.dart
│   │   ├── app_text.dart
│   │   ├── app_form_field.dart
│   │   ├── app_loading_indicator.dart
│   │   ├── app_error_widget.dart
│   │   └── [other atomic widgets]
│   ├── composite/
│   │   ├── app_search_bar.dart
│   │   ├── app_pagination_bar.dart
│   │   ├── app_empty_state.dart
│   │   └── [other composite widgets]
│   └── feature/
│       ├── [entity]/
│       │   ├── [entity]_list_view.dart
│       │   ├── [entity]_form_widget.dart
│       │   ├── [entity]_details_view.dart
│       │   └── widgets/
│       │       └── [entity]_list_tile.dart
└── main.dart
```

---

## 1. Mock Data Layer Structure

### Mock Service Pattern

```dart
/// lib/data/mock/mock_user_service.dart
class MockUserService {
  // In-memory storage
  static final List<UserDto> _users = [...]; // Pre-populated mock data
  
  // Simulate network delay
  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 500));
  }
  
  /// Get paginated list
  static Future<PagedResultDto<UserDto>> getList({
    int skipCount = 0,
    int maxResultCount = 10,
    String? sorting,
    String? filter,
  }) async {
    await _simulateDelay();
    
    // Apply filtering
    var filtered = _users;
    if (filter != null && filter.isNotEmpty) {
      filtered = _users.where((user) =>
        user.userName.toLowerCase().contains(filter.toLowerCase()) ||
        user.email.toLowerCase().contains(filter.toLowerCase())
      ).toList();
    }
    
    // Apply sorting
    if (sorting != null) {
      final parts = sorting.split(' ');
      final field = parts[0];
      final ascending = parts.length < 2 || parts[1].toUpperCase() == 'ASC';
      
      filtered.sort((a, b) {
        int comparison = 0;
        switch (field.toLowerCase()) {
          case 'username':
            comparison = a.userName.compareTo(b.userName);
            break;
          case 'email':
            comparison = a.email.compareTo(b.email);
            break;
          case 'creationtime':
            comparison = a.creationTime.compareTo(b.creationTime);
            break;
        }
        return ascending ? comparison : -comparison;
      });
    }
    
    // Apply pagination
    final paginatedItems = filtered
        .skip(skipCount)
        .take(maxResultCount)
        .toList();
    
    return PagedResultDto(
      items: paginatedItems,
      totalCount: filtered.length,
    );
  }
  
  /// Get single item
  static Future<UserDto> get(String id) async {
    await _simulateDelay();
    
    try {
      return _users.firstWhere((user) => user.id == id);
    } catch (e) {
      throw Exception('User not found');
    }
  }
  
  /// Create new item
  static Future<UserDto> create(CreateUpdateUserDto input) async {
    await _simulateDelay();
    
    // Validate
    if (input.userName.isEmpty) {
      throw ValidationException('User name is required');
    }
    if (_users.any((u) => u.userName == input.userName)) {
      throw ValidationException('User name already exists');
    }
    
    final newUser = UserDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      userName: input.userName,
      email: input.email,
      phoneNumber: input.phoneNumber,
      isActive: input.isActive,
      roleNames: input.roleNames,
      creationTime: DateTime.now(),
    );
    
    _users.add(newUser);
    return newUser;
  }
  
  /// Update existing item
  static Future<UserDto> update(String id, CreateUpdateUserDto input) async {
    await _simulateDelay();
    
    final index = _users.indexWhere((user) => user.id == id);
    if (index == -1) {
      throw Exception('User not found');
    }
    
    // Validate
    if (input.userName.isEmpty) {
      throw ValidationException('User name is required');
    }
    if (_users.any((u) => u.userName == input.userName && u.id != id)) {
      throw ValidationException('User name already exists');
    }
    
    final updatedUser = _users[index].copyWith(
      userName: input.userName,
      email: input.email,
      phoneNumber: input.phoneNumber,
      isActive: input.isActive,
      roleNames: input.roleNames,
      lastModificationTime: DateTime.now(),
    );
    
    _users[index] = updatedUser;
    return updatedUser;
  }
  
  /// Delete item
  static Future<void> delete(String id) async {
    await _simulateDelay();
    
    final removed = _users.removeWhere((user) => user.id == id);
    if (removed == 0) {
      throw Exception('User not found');
    }
  }
}

/// Custom exception for validation errors
class ValidationException implements Exception {
  final String message;
  final Map<String, String>? fieldErrors;
  
  ValidationException(this.message, {this.fieldErrors});
  
  @override
  String toString() => message;
}
```

---

## 2. DTO Models

Generate complete DTO classes with:
- All properties from the feature description
- `fromJson` and `toJson` methods
- `copyWith` method for immutability
- Equality override
- Freezed or manual implementation

```dart
/// lib/data/models/user_dto.dart
import 'package:freezed_annotation/freezed_annotation.dart';

part 'user_dto.freezed.dart';
part 'user_dto.g.dart';

@freezed
class UserDto with _$UserDto {
  const factory UserDto({
    required String id,
    required String userName,
    required String email,
    String? phoneNumber,
    required bool isActive,
    required List<String> roleNames,
    required DateTime creationTime,
    DateTime? lastModificationTime,
  }) = _UserDto;

  factory UserDto.fromJson(Map<String, dynamic> json) =>
      _$UserDtoFromJson(json);
}

@freezed
class CreateUpdateUserDto with _$CreateUpdateUserDto {
  const factory CreateUpdateUserDto({
    required String userName,
    required String email,
    String? phoneNumber,
    required String password,
    @Default(true) bool isActive,
    @Default([]) List<String> roleNames,
  }) = _CreateUpdateUserDto;

  factory CreateUpdateUserDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateUserDtoFromJson(json);
}

@freezed
class PagedResultDto<T> with _$PagedResultDto<T> {
  const factory PagedResultDto({
    required List<T> items,
    required int totalCount,
  }) = _PagedResultDto<T>;

  factory PagedResultDto.fromJson(
    Map<String, dynamic> json,
    T Function(Object?) fromJsonT,
  ) =>
      _$PagedResultDtoFromJson(json, fromJsonT);
}
```

---

## 3. Riverpod Providers (Using Mock Data)

### 3.1 Service Provider

```dart
/// lib/providers/user_service_provider.dart
import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/mock/mock_user_service.dart';

part 'user_service_provider.g.dart';

@riverpod
MockUserService userService(UserServiceRef ref) {
  return MockUserService();
}
```

### 3.2 List Provider

```dart
/// lib/providers/user_list_provider.dart
import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/user_dto.dart';
import '../data/models/paged_result_dto.dart';
import '../data/mock/mock_user_service.dart';

part 'user_list_provider.g.dart';

@riverpod
class UserList extends _$UserList {
  int _skipCount = 0;
  int _maxResultCount = 10;
  String? _sorting;
  String? _filter;

  @override
  Future<PagedResultDto<UserDto>> build() async {
    return _loadUsers();
  }

  Future<PagedResultDto<UserDto>> _loadUsers() async {
    return await MockUserService.getList(
      skipCount: _skipCount,
      maxResultCount: _maxResultCount,
      sorting: _sorting,
      filter: _filter,
    );
  }

  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadUsers());
  }

  Future<void> nextPage() async {
    final currentData = state.value;
    if (currentData == null) return;

    if (_skipCount + _maxResultCount < currentData.totalCount) {
      _skipCount += _maxResultCount;
      await refresh();
    }
  }

  Future<void> previousPage() async {
    if (_skipCount > 0) {
      _skipCount = (_skipCount - _maxResultCount).clamp(0, double.infinity).toInt();
      await refresh();
    }
  }

  Future<void> search(String? query) async {
    _filter = query;
    _skipCount = 0;
    await refresh();
  }

  Future<void> sort(String field, bool ascending) async {
    _sorting = '$field ${ascending ? 'ASC' : 'DESC'}';
    await refresh();
  }

  Future<void> setPageSize(int size) async {
    _maxResultCount = size;
    _skipCount = 0;
    await refresh();
  }

  int get currentPage => (_skipCount / _maxResultCount).floor() + 1;
  
  int getTotalPages(int? totalCount) {
    if (totalCount == null) return 0;
    return (totalCount / _maxResultCount).ceil();
  }
}
```

### 3.3 Single Entity Provider

```dart
/// lib/providers/user_provider.dart
import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/user_dto.dart';
import '../data/mock/mock_user_service.dart';

part 'user_provider.g.dart';

@riverpod
class User extends _$User {
  @override
  Future<UserDto> build(String id) async {
    return await MockUserService.get(id);
  }

  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => MockUserService.get(id));
  }
}
```

### 3.4 Form Provider

```dart
/// lib/providers/user_form_provider.dart
import 'package:riverpod_annotation/riverpod_annotation.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
import '../data/models/user_dto.dart';
import '../data/mock/mock_user_service.dart';
import 'user_provider.dart';
import 'user_list_provider.dart';

part 'user_form_provider.g.dart';
part 'user_form_provider.freezed.dart';

@riverpod
class UserForm extends _$UserForm {
  @override
  UserFormState build({String? userId}) {
    if (userId != null) {
      ref.listen(userProvider(userId), (previous, next) {
        next.whenData((user) {
          state = state.copyWith(
            userName: user.userName,
            email: user.email,
            phoneNumber: user.phoneNumber,
            isActive: user.isActive,
            roleNames: user.roleNames,
          );
        });
      });
    }

    return const UserFormState();
  }

  void updateUserName(String value) {
    state = state.copyWith(
      userName: value,
      isDirty: true,
      errors: {...state.errors}..remove('userName'),
    );
  }

  void updateEmail(String value) {
    state = state.copyWith(
      email: value,
      isDirty: true,
      errors: {...state.errors}..remove('email'),
    );
  }

  void updatePhoneNumber(String? value) {
    state = state.copyWith(
      phoneNumber: value,
      isDirty: true,
    );
  }

  void updatePassword(String value) {
    state = state.copyWith(
      password: value,
      isDirty: true,
      errors: {...state.errors}..remove('password'),
    );
  }

  void updateIsActive(bool value) {
    state = state.copyWith(isActive: value, isDirty: true);
  }

  void updateRoleNames(List<String> value) {
    state = state.copyWith(roleNames: value, isDirty: true);
  }

  bool validate() {
    final errors = <String, String>{};

    if (state.userName.isEmpty) {
      errors['userName'] = 'User name is required';
    } else if (state.userName.length < 3) {
      errors['userName'] = 'User name must be at least 3 characters';
    }

    if (state.email.isEmpty) {
      errors['email'] = 'Email is required';
    } else if (!_isValidEmail(state.email)) {
      errors['email'] = 'Invalid email format';
    }

    if (userId == null && state.password.isEmpty) {
      errors['password'] = 'Password is required';
    } else if (state.password.isNotEmpty && state.password.length < 6) {
      errors['password'] = 'Password must be at least 6 characters';
    }

    state = state.copyWith(errors: errors);
    return errors.isEmpty;
  }

  Future<UserDto?> submit() async {
    if (!validate()) return null;

    state = state.copyWith(isSubmitting: true, submitError: null);

    try {
      final dto = CreateUpdateUserDto(
        userName: state.userName,
        email: state.email,
        phoneNumber: state.phoneNumber,
        password: state.password,
        isActive: state.isActive,
        roleNames: state.roleNames,
      );

      final UserDto result;

      if (userId != null) {
        result = await MockUserService.update(userId!, dto);
        ref.invalidate(userProvider(userId!));
      } else {
        result = await MockUserService.create(dto);
      }

      ref.invalidate(userListProvider);
      state = const UserFormState();

      return result;
    } on ValidationException catch (e) {
      state = state.copyWith(
        isSubmitting: false,
        submitError: e.message,
        errors: e.fieldErrors ?? {},
      );
      return null;
    } catch (e) {
      state = state.copyWith(
        isSubmitting: false,
        submitError: e.toString(),
      );
      return null;
    }
  }

  void reset() {
    state = const UserFormState();
  }

  bool _isValidEmail(String email) {
    return RegExp(r'^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$').hasMatch(email);
  }
}

@freezed
class UserFormState with _$UserFormState {
  const factory UserFormState({
    @Default('') String userName,
    @Default('') String email,
    String? phoneNumber,
    @Default('') String password,
    @Default(true) bool isActive,
    @Default([]) List<String> roleNames,
    @Default(false) bool isDirty,
    @Default(false) bool isSubmitting,
    @Default({}) Map<String, String> errors,
    String? submitError,
  }) = _UserFormState;
}
```

### 3.5 Delete Provider

```dart
/// lib/providers/user_delete_provider.dart
import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/mock/mock_user_service.dart';
import 'user_list_provider.dart';
import 'user_provider.dart';

part 'user_delete_provider.g.dart';

@riverpod
Future<void> deleteUser(DeleteUserRef ref, String userId) async {
  await MockUserService.delete(userId);
  
  ref.invalidate(userListProvider);
  ref.invalidate(userProvider(userId));
}
```

---

## 4. Theme System

Generate a complete theme system:

```dart
/// lib/core/theme/app_colors.dart
import 'package:flutter/material.dart';

class AppColors {
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
  static const error = Color(0xFFD32F2F);
  static const success = Color(0xFF388E3C);
  static const warning = Color(0xFFF57C00);
  static const info = Color(0xFF1976D2);
  
  // Text colors
  static const textPrimary = Color(0xFF212121);
  static const textSecondary = Color(0xFF757575);
  static const textDisabled = Color(0xFFBDBDBD);
  static const textHint = Color(0xFF9E9E9E);
  
  // Border colors
  static const border = Color(0xFFE0E0E0);
  static const divider = Color(0xFFBDBDBD);
}

/// lib/core/theme/app_text_styles.dart
import 'package:flutter/material.dart';
import 'app_colors.dart';

class AppTextStyles {
  // Headings
  static const h1 = TextStyle(
    fontSize: 32,
    fontWeight: FontWeight.bold,
    color: AppColors.textPrimary,
    height: 1.2,
  );
  
  static const h2 = TextStyle(
    fontSize: 24,
    fontWeight: FontWeight.bold,
    color: AppColors.textPrimary,
    height: 1.3,
  );
  
  static const h3 = TextStyle(
    fontSize: 20,
    fontWeight: FontWeight.w600,
    color: AppColors.textPrimary,
    height: 1.4,
  );
  
  // Body text
  static const bodyLarge = TextStyle(
    fontSize: 16,
    fontWeight: FontWeight.normal,
    color: AppColors.textPrimary,
    height: 1.5,
  );
  
  static const bodyMedium = TextStyle(
    fontSize: 14,
    fontWeight: FontWeight.normal,
    color: AppColors.textPrimary,
    height: 1.5,
  );
  
  static const bodySmall = TextStyle(
    fontSize: 12,
    fontWeight: FontWeight.normal,
    color: AppColors.textSecondary,
    height: 1.5,
  );
  
  // Labels
  static const label = TextStyle(
    fontSize: 14,
    fontWeight: FontWeight.w500,
    color: AppColors.textPrimary,
    height: 1.4,
  );
  
  // Button text
  static const button = TextStyle(
    fontSize: 14,
    fontWeight: FontWeight.w600,
    height: 1.2,
  );
}

/// lib/core/theme/app_spacing.dart
class AppSpacing {
  static const double xxs = 2.0;
  static const double xs = 4.0;
  static const double sm = 8.0;
  static const double md = 16.0;
  static const double lg = 24.0;
  static const double xl = 32.0;
  static const double xxl = 48.0;
}
```

---

## 5. Atomic Widgets

Generate reusable atomic widgets:

```dart
/// lib/widgets/atomic/app_button.dart
import 'package:flutter/material.dart';
import '../../core/theme/app_colors.dart';
import '../../core/theme/app_text_styles.dart';
import '../../core/theme/app_spacing.dart';

class AppButton extends StatelessWidget {
  final String label;
  final VoidCallback? onPressed;
  final bool isLoading;
  final bool isFullWidth;
  final ButtonVariant variant;
  final ButtonSize size;
  final IconData? icon;

  const AppButton({
    super.key,
    required this.label,
    this.onPressed,
    this.isLoading = false,
    this.isFullWidth = true,
    this.variant = ButtonVariant.primary,
    this.size = ButtonSize.medium,
    this.icon,
  });

  @override
  Widget build(BuildContext context) {
    final bool isEnabled = onPressed != null && !isLoading;
    
    return SizedBox(
      width: isFullWidth ? double.infinity : null,
      height: _getHeight(),
      child: ElevatedButton(
        onPressed: isEnabled ? onPressed : null,
        style: _getButtonStyle(),
        child: isLoading
            ? SizedBox(
                height: 20,
                width: 20,
                child: CircularProgressIndicator(
                  strokeWidth: 2,
                  valueColor: AlwaysStoppedAnimation<Color>(
                    variant == ButtonVariant.primary
                        ? Colors.white
                        : AppColors.primary,
                  ),
                ),
              )
            : Row(
                mainAxisSize: MainAxisSize.min,
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  if (icon != null) ...[
                    Icon(icon, size: _getIconSize()),
                    SizedBox(width: AppSpacing.sm),
                  ],
                  Text(label, style: _getTextStyle()),
                ],
              ),
      ),
    );
  }

  ButtonStyle _getButtonStyle() {
    return ElevatedButton.styleFrom(
      backgroundColor: _getBackgroundColor(),
      foregroundColor: _getForegroundColor(),
      disabledBackgroundColor: AppColors.border,
      disabledForegroundColor: AppColors.textDisabled,
      elevation: variant == ButtonVariant.outlined ? 0 : 2,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(8),
        side: variant == ButtonVariant.outlined
            ? BorderSide(color: _getBorderColor())
            : BorderSide.none,
      ),
      padding: EdgeInsets.symmetric(
        horizontal: _getHorizontalPadding(),
        vertical: AppSpacing.sm,
      ),
    );
  }

  Color _getBackgroundColor() {
    switch (variant) {
      case ButtonVariant.primary:
        return AppColors.primary;
      case ButtonVariant.secondary:
        return AppColors.secondary;
      case ButtonVariant.outlined:
        return Colors.transparent;
      case ButtonVariant.text:
        return Colors.transparent;
      case ButtonVariant.danger:
        return AppColors.error;
    }
  }

  Color _getForegroundColor() {
    switch (variant) {
      case ButtonVariant.primary:
      case ButtonVariant.secondary:
      case ButtonVariant.danger:
        return Colors.white;
      case ButtonVariant.outlined:
      case ButtonVariant.text:
        return AppColors.primary;
    }
  }

  Color _getBorderColor() {
    return AppColors.primary;
  }

  TextStyle _getTextStyle() {
    return AppTextStyles.button.copyWith(
      fontSize: size == ButtonSize.small ? 12 : 14,
    );
  }

  double _getHeight() {
    switch (size) {
      case ButtonSize.small:
        return 36;
      case ButtonSize.medium:
        return 44;
      case ButtonSize.large:
        return 52;
    }
  }

  double _getHorizontalPadding() {
    switch (size) {
      case ButtonSize.small:
        return AppSpacing.md;
      case ButtonSize.medium:
        return AppSpacing.lg;
      case ButtonSize.large:
        return AppSpacing.xl;
    }
  }

  double _getIconSize() {
    switch (size) {
      case ButtonSize.small:
        return 16;
      case ButtonSize.medium:
        return 20;
      case ButtonSize.large:
        return 24;
    }
  }
}

enum ButtonVariant { primary, secondary, outlined, text, danger }
enum ButtonSize { small, medium, large }

/// lib/widgets/atomic/app_form_field.dart
import 'package:flutter/material.dart';
import '../../core/theme/app_colors.dart';
import '../../core/theme/app_text_styles.dart';
import '../../core/theme/app_spacing.dart';

class AppFormField extends StatelessWidget {
  final String? label;
  final String? hint;
  final String? initialValue;
  final String? errorText;
  final TextInputType keyboardType;
  final bool obscureText;
  final bool enabled;
  final int? maxLines;
  final ValueChanged<String>? onChanged;
  final TextEditingController? controller;

  const AppFormField({
    super.key,
    this.label,
    this.hint,
    this.initialValue,
    this.errorText,
    this.keyboardType = TextInputType.text,
    this.obscureText = false,
    this.enabled = true,
    this.maxLines = 1,
    this.onChanged,
    this.controller,
  });

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        if (label != null) ...[
          Text(label!, style: AppTextStyles.label),
          SizedBox(height: AppSpacing.xs),
        ],
        TextFormField(
          controller: controller,
          initialValue: controller == null ? initialValue : null,
          keyboardType: keyboardType,
          obscureText: obscureText,
          enabled: enabled,
          maxLines: obscureText ? 1 : maxLines,
          style: AppTextStyles.bodyMedium,
          decoration: InputDecoration(
            hintText: hint,
            errorText: errorText,
            filled: true,
            fillColor: enabled ? Colors.white : AppColors.background,
            contentPadding: EdgeInsets.symmetric(
              horizontal: AppSpacing.md,
              vertical: AppSpacing.md,
            ),
            border: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8),
              borderSide: BorderSide(color: AppColors.border),
            ),
            enabledBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8),
              borderSide: BorderSide(color: AppColors.border),
            ),
            focusedBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8),
              borderSide: BorderSide(color: AppColors.primary, width: 2),
            ),
            errorBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8),
              borderSide: BorderSide(color: AppColors.error),
            ),
            disabledBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8),
              borderSide: BorderSide(color: AppColors.border),
            ),
          ),
          onChanged: onChanged,
        ),
      ],
    );
  }
}

// Add more atomic widgets: AppText, AppLoadingIndicator, AppErrorWidget, etc.
```

---

## 6. Feature Widgets (Complete Screens)

Generate complete feature widgets with full CRUD functionality:

```dart
/// lib/widgets/feature/user/user_list_view.dart
import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../providers/user_list_provider.dart';
import '../../../providers/user_delete_provider.dart';
import '../../atomic/app_button.dart';
import '../../atomic/app_loading_indicator.dart';
import '../../atomic/app_error_widget.dart';
import '../../composite/app_empty_state.dart';
import '../../composite/app_pagination_bar.dart';
import 'widgets/user_list_tile.dart';
import 'user_form_screen.dart';
import 'user_details_screen.dart';

class UserListView extends ConsumerWidget {
  const UserListView({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final usersAsync = ref.watch(userListProvider);
    final userListNotifier = ref.watch(userListProvider.notifier);

    return Scaffold(
      appBar: AppBar(
        title: const Text('Users'),
        actions: [
          IconButton(
            icon: const Icon(Icons.add),
            onPressed: () => _navigateToCreateUser(context),
          ),
        ],
      ),
      body: Column(
        children: [
          // Search bar
          Padding(
            padding: const EdgeInsets.all(16.0),
            child: TextField(
              decoration: InputDecoration(
                hintText: 'Search users...',
                prefixIcon: const Icon(Icons.search),
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(8),
                ),
              ),
              onChanged: (query) {
                userListNotifier.search(query.isEmpty ? null : query);
              },
            ),
          ),

          // User list
          Expanded(
            child: usersAsync.when(
              data: (pagedResult) {
                if (pagedResult.items.isEmpty) {
                  return AppEmptyState(
                    message: 'No users found',
                    action: AppButton(
                      label: 'Create User',
                      onPressed: () => _navigateToCreateUser(context),
                    ),
                  );
                }

                return ListView.builder(
                  itemCount: pagedResult.items.length,
                  itemBuilder: (context, index) {
                    final user = pagedResult.items[index];
                    return UserListTile(
                      user: user,
                      onTap: () => _navigateToUserDetails(context, user.id),
                      onDelete: () => _confirmDelete(context, ref, user.id, user.userName),
                    );
                  },
                );
              },
              loading: () => const Center(child: CircularProgressIndicator()),
              error: (error, stack) => AppErrorWidget(
                error: error.toString(),
                onRetry: () => ref.invalidate(userListProvider),
              ),
            ),
          ),

          // Pagination
          if (usersAsync.hasValue)
            AppPaginationBar(
              currentPage: userListNotifier.currentPage,
              totalPages: userListNotifier.getTotalPages(usersAsync.value?.totalCount),
              onPreviousPage: () => userListNotifier.previousPage(),
              onNextPage: () => userListNotifier.nextPage(),
            ),
        ],
      ),
    );
  }

  void _navigateToCreateUser(BuildContext context) {
    Navigator.of(context).push(
      MaterialPageRoute(
        builder: (_) => const UserFormScreen(),
      ),
    );
  }

  void _navigateToUserDetails(BuildContext context, String userId) {
    Navigator.of(context).push(
      MaterialPageRoute(
        builder: (_) => UserDetailsScreen(userId: userId),
      ),
    );
  }

  Future<void> _confirmDelete(
    BuildContext context,
    WidgetRef ref,
    String userId,
    String userName,
  ) async {
    final confirmed = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Delete User'),
        content: Text('Are you sure you want to delete $userName?'),
        actions: [
          TextButton(
            onPressed: () => Navigator.of(context).pop(false),
            child: const Text('Cancel'),
          ),
          TextButton(
            onPressed: () => Navigator.of(context).pop(true),
            style: TextButton.styleFrom(foregroundColor: Colors.red),
            child: const Text('Delete'),
          ),
        ],
      ),
    );

    if (confirmed == true && context.mounted) {
      try {
        await ref.read(deleteUserProvider(userId).future);
        if (context.mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text('User deleted successfully')),
          );
        }
      } catch (e) {
        if (context.mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(content: Text('Failed to delete user: $e')),
          );
        }
      }
    }
  }
}
```

---

## Instructions for Code Generation

When I provide a feature description, generate:

1. **Complete mock data** with realistic sample data (at least 10-20 items)
2. **All DTO models** with freezed annotations
3. **All Riverpod providers** following the patterns above
4. **All required atomic widgets** used by the feature
5. **All required composite widgets** used by the feature
6. **Complete feature widgets** (list, form, details views)
7. **Navigation integration** between screens
8. **Full CRUD operations** using mock data
9. **Error handling** and loading states
10. **Validation** for forms

### Code Requirements:

- ✅ Use latest Riverpod syntax (riverpod_annotation)
- ✅ Use Freezed for immutable models
- ✅ Include null safety
- ✅ Add comprehensive comments
- ✅ Follow Flutter/Dart style guide
- ✅ Include error handling
- ✅ Implement loading states
- ✅ Add form validation
- ✅ Use theme system consistently
- ✅ Make code production-ready
- ✅ NO placeholder comments like "// Add more fields"
- ✅ NO incomplete implementations
