# Comprehensive Flutter UI & State Management Documentation

## Context
You are creating detailed documentation for a Flutter application with ABP Framework backend. The documentation covers UI components (atomic, composite, feature widgets) and Riverpod state management that integrates with ABP application services and DTOs.

## Architecture Overview
1. **ABP Application Layer** - Backend services and DTOs
2. **Riverpod State Management** - Frontend state mapped to ABP DTOs
3. **UI Hierarchy** - Atomic → Composite → Feature widgets
4. **Theme System** - Centralized design tokens

---

## Part 1: ABP to Riverpod State Management Documentation

### 1. ABP Application Service Analysis

For each ABP Application Service, document:

**Service Interface:** `IUserAppService`

**Standard ABP Methods:**
- `GetListAsync(PagedAndSortedResultRequestDto input)` → Returns `PagedResultDto<UserDto>`
- `GetAsync(Guid id)` → Returns `UserDto`
- `CreateAsync(CreateUpdateUserDto input)` → Returns `UserDto`
- `UpdateAsync(Guid id, CreateUpdateUserDto input)` → Returns `UserDto`
- `DeleteAsync(Guid id)` → Returns `void`

**Custom Methods:** List any additional service methods

**DTOs Used:**
- `UserDto` (output)
- `CreateUpdateUserDto` (input)
- Any custom DTOs

### 2. DTO Documentation

For each DTO, provide:

**DTO Structure:**
```dart
// Output DTO
class UserDto {
  final String id;
  final String userName;
  final String email;
  final String? phoneNumber;
  final bool isActive;
  final DateTime creationTime;
  final DateTime? lastModificationTime;
  
  const UserDto({
    required this.id,
    required this.userName,
    required this.email,
    this.phoneNumber,
    required this.isActive,
    required this.creationTime,
    this.lastModificationTime,
  });
  
  factory UserDto.fromJson(Map<String, dynamic> json) => ...;
  Map<String, dynamic> toJson() => ...;
}

// Input DTO
class CreateUpdateUserDto {
  final String userName;
  final String email;
  final String? phoneNumber;
  final String password;
  final bool isActive;
  final List<String> roleNames;
  
  const CreateUpdateUserDto({
    required this.userName,
    required this.email,
    this.phoneNumber,
    required this.password,
    this.isActive = true,
    this.roleNames = const [],
  });
  
  Map<String, dynamic> toJson() => ...;
}
```

**Validation Rules:**
- Required fields
- String length constraints
- Format requirements (email, phone)
- Business rules

### 3. Riverpod Provider Architecture

Document providers in the following hierarchy:

#### 3.1 Service Provider (Singleton)

```dart
/// Provides the ABP User Application Service
/// 
/// This is a singleton provider that creates the HTTP client
/// for the User service and handles API communication.
/// 
/// Usage: ref.read(userServiceProvider)
@riverpod
IUserAppService userService(UserServiceRef ref) {
  final httpClient = ref.watch(httpClientProvider);
  final baseUrl = ref.watch(configProvider).apiBaseUrl;
  
  return UserAppService(
    httpClient: httpClient,
    baseUrl: baseUrl,
  );
}
```

#### 3.2 List Provider (StateNotifier)

```dart
/// Manages the paginated list of users from ABP backend
/// 
/// State: AsyncValue<PagedResultDto<UserDto>>
/// 
/// Features:
/// - Pagination support
/// - Sorting support
/// - Search/filtering
/// - Auto-refresh on CRUD operations
/// - Loading, error, and success states
/// 
/// Methods:
/// - loadUsers({int skipCount, int maxResultCount, String? sorting, String? filter})
/// - refresh()
/// - nextPage()
/// - previousPage()
/// - search(String query)
/// - sort(String field, bool ascending)
@riverpod
class UserList extends _$UserList {
  // Pagination state
  int _skipCount = 0;
  int _maxResultCount = 10;
  String? _sorting;
  String? _filter;
  
  @override
  FutureOr<PagedResultDto<UserDto>> build() {
    return _loadUsers();
  }
  
  Future<PagedResultDto<UserDto>> _loadUsers() async {
    final service = ref.read(userServiceProvider);
    
    return await service.getList(
      PagedAndSortedResultRequestDto(
        skipCount: _skipCount,
        maxResultCount: _maxResultCount,
        sorting: _sorting,
        filter: _filter,
      ),
    );
  }
  
  /// Refreshes the current page
  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadUsers());
  }
  
  /// Loads the next page of results
  Future<void> nextPage() async {
    final currentData = state.value;
    if (currentData == null) return;
    
    if (_skipCount + _maxResultCount < currentData.totalCount) {
      _skipCount += _maxResultCount;
      await refresh();
    }
  }
  
  /// Loads the previous page
  Future<void> previousPage() async {
    if (_skipCount > 0) {
      _skipCount = (_skipCount - _maxResultCount).clamp(0, double.infinity).toInt();
      await refresh();
    }
  }
  
  /// Searches/filters users
  Future<void> search(String? query) async {
    _filter = query;
    _skipCount = 0; // Reset to first page
    await refresh();
  }
  
  /// Sorts users by field
  Future<void> sort(String field, bool ascending) async {
    _sorting = '$field ${ascending ? 'ASC' : 'DESC'}';
    await refresh();
  }
  
  /// Changes page size
  Future<void> setPageSize(int size) async {
    _maxResultCount = size;
    _skipCount = 0;
    await refresh();
  }
}

// Helper provider for current page number
@riverpod
int userListCurrentPage(UserListCurrentPageRef ref) {
  final listNotifier = ref.watch(userListProvider.notifier);
  return (listNotifier._skipCount / listNotifier._maxResultCount).floor() + 1;
}

// Helper provider for total pages
@riverpod
int userListTotalPages(UserListTotalPagesRef ref) {
  final data = ref.watch(userListProvider).value;
  final listNotifier = ref.watch(userListProvider.notifier);
  
  if (data == null) return 0;
  return (data.totalCount / listNotifier._maxResultCount).ceil();
}
```

#### 3.3 Single Entity Provider (Provider Family)

```dart
/// Fetches a single user by ID
/// 
/// This is a provider family that creates a separate provider
/// instance for each user ID.
/// 
/// State: AsyncValue<UserDto>
/// 
/// Usage: ref.watch(userProvider(userId))
/// 
/// Features:
/// - Automatic caching by ID
/// - Auto-refresh after update
/// - Integrated error handling
@riverpod
class User extends _$User {
  @override
  FutureOr<UserDto> build(String id) async {
    final service = ref.read(userServiceProvider);
    return await service.get(id);
  }
  
  /// Refreshes this user's data
  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() async {
      final service = ref.read(userServiceProvider);
      return await service.get(id);
    });
  }
}
```

#### 3.4 Create/Update Form Provider (StateNotifier)

```dart
/// Manages form state for creating or updating a user
/// 
/// State: UserFormState
/// 
/// Features:
/// - Field validation
/// - Dirty state tracking
/// - Submission handling
/// - Error display
/// - Integration with ABP validation errors
@riverpod
class UserForm extends _$UserForm {
  @override
  UserFormState build({String? userId}) {
    // If userId provided, load existing user for editing
    if (userId != null) {
      ref.listen(userProvider(userId), (previous, next) {
        next.whenData((user) {
          state = state.copyWith(
            userName: user.userName,
            email: user.email,
            phoneNumber: user.phoneNumber,
            isActive: user.isActive,
          );
        });
      });
    }
    
    return const UserFormState();
  }
  
  /// Updates userName field
  void updateUserName(String value) {
    state = state.copyWith(
      userName: value,
      isDirty: true,
      errors: {...state.errors}..remove('userName'),
    );
  }
  
  /// Updates email field
  void updateEmail(String value) {
    state = state.copyWith(
      email: value,
      isDirty: true,
      errors: {...state.errors}..remove('email'),
    );
  }
  
  /// Updates phoneNumber field
  void updatePhoneNumber(String? value) {
    state = state.copyWith(
      phoneNumber: value,
      isDirty: true,
      errors: {...state.errors}..remove('phoneNumber'),
    );
  }
  
  /// Updates password field
  void updatePassword(String value) {
    state = state.copyWith(
      password: value,
      isDirty: true,
      errors: {...state.errors}..remove('password'),
    );
  }
  
  /// Updates isActive field
  void updateIsActive(bool value) {
    state = state.copyWith(
      isActive: value,
      isDirty: true,
    );
  }
  
  /// Updates roleNames field
  void updateRoleNames(List<String> value) {
    state = state.copyWith(
      roleNames: value,
      isDirty: true,
    );
  }
  
  /// Validates the form
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
  
  /// Submits the form (create or update)
  Future<UserDto?> submit() async {
    if (!validate()) {
      return null;
    }
    
    state = state.copyWith(isSubmitting: true, submitError: null);
    
    try {
      final service = ref.read(userServiceProvider);
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
        // Update existing user
        result = await service.update(userId!, dto);
        
        // Invalidate the single user provider
        ref.invalidate(userProvider(userId!));
      } else {
        // Create new user
        result = await service.create(dto);
      }
      
      // Invalidate the user list to refresh
      ref.invalidate(userListProvider);
      
      // Reset form state
      state = const UserFormState();
      
      return result;
    } on AbpException catch (e) {
      // Handle ABP validation errors
      final errors = <String, String>{};
      
      if (e.validationErrors != null) {
        for (final error in e.validationErrors!) {
          // Map ABP field names to form field names
          final fieldName = _mapAbpFieldName(error.members.firstOrNull);
          if (fieldName != null) {
            errors[fieldName] = error.message;
          }
        }
      }
      
      state = state.copyWith(
        isSubmitting: false,
        submitError: e.message,
        errors: errors,
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
  
  /// Resets the form
  void reset() {
    state = const UserFormState();
  }
  
  bool _isValidEmail(String email) {
    return RegExp(r'^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$').hasMatch(email);
  }
  
  String? _mapAbpFieldName(String? abpFieldName) {
    // Map ABP DTO field names to form field names (usually camelCase)
    return abpFieldName?.substring(0, 1).toLowerCase() + 
           (abpFieldName?.substring(1) ?? '');
  }
}

/// Form state model
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

#### 3.5 Delete Operation Provider

```dart
/// Handles user deletion
/// 
/// Usage:
/// ```dart
/// await ref.read(userDeleteProvider(userId).future);
/// ```
@riverpod
class UserDelete extends _$UserDelete {
  @override
  FutureOr<void> build(String id) {
    // Don't auto-execute
    return null;
  }
  
  /// Executes the delete operation
  Future<void> execute() async {
    state = const AsyncValue.loading();
    
    state = await AsyncValue.guard(() async {
      final service = ref.read(userServiceProvider);
      await service.delete(id);
      
      // Invalidate related providers
      ref.invalidate(userListProvider);
      ref.invalidate(userProvider(id));
    });
  }
}

// Or as a simple function provider
@riverpod
Future<void> deleteUser(DeleteUserRef ref, String userId) async {
  final service = ref.read(userServiceProvider);
  await service.delete(userId);
  
  // Invalidate cache
  ref.invalidate(userListProvider);
  ref.invalidate(userProvider(userId));
}
```

#### 3.6 Lookup/Reference Data Providers

```dart
/// Provides list of roles for user assignment
/// 
/// This is typically used in dropdowns or multi-select widgets
@riverpod
Future<List<RoleDto>> roleOptions(RoleOptionsRef ref) async {
  final service = ref.read(roleServiceProvider);
  final result = await service.getList(
    PagedAndSortedResultRequestDto(maxResultCount: 100),
  );
  return result.items;
}

/// Provides list of tenants (for host users)
@riverpod
Future<List<TenantDto>> tenantOptions(TenantOptionsRef ref) async {
  final service = ref.read(tenantServiceProvider);
  final result = await service.getList(
    PagedAndSortedResultRequestDto(maxResultCount: 100),
  );
  return result.items;
}
```

#### 3.7 Search/Filter Providers

```dart
/// Manages search query state
@riverpod
class UserSearchQuery extends _$UserSearchQuery {
  @override
  String build() => '';
  
  void update(String query) {
    state = query;
  }
  
  void clear() {
    state = '';
  }
}

/// Filtered user list based on search query
@riverpod
Future<List<UserDto>> filteredUsers(FilteredUsersRef ref) async {
  final query = ref.watch(userSearchQueryProvider);
  final service = ref.read(userServiceProvider);
  
  final result = await service.getList(
    PagedAndSortedResultRequestDto(
      maxResultCount: 100,
      filter: query,
    ),
  );
  
  return result.items;
}
```

### 4. Provider Dependency Map

Document how providers depend on each other:

```
userServiceProvider (Service Layer)
    ↓
├── userListProvider (List Management)
│   ├── Used by: UserListView widget
│   ├── Invalidated by: userFormProvider, deleteUser
│   └── Exports: pagination state, search, sort methods
│
├── userProvider(id) (Single Entity)
│   ├── Used by: UserDetailsView, UserEditForm
│   ├── Invalidated by: userFormProvider (on update)
│   └── Cached by: family parameter (id)
│
├── userFormProvider (Form State)
│   ├── Used by: UserFormWidget
│   ├── Reads from: userProvider (for editing)
│   ├── Invalidates: userListProvider, userProvider
│   └── Handles: validation, submission, ABP errors
│
├── deleteUser(id) (Delete Operation)
│   ├── Used by: UserListView, UserDetailsView
│   ├── Invalidates: userListProvider, userProvider
│   └── Handles: confirmation, error display
│
└── roleOptionsProvider (Reference Data)
    ├── Used by: UserFormWidget (role selection)
    └── Cached: until invalidated
```

### 5. State Management Patterns

Document common patterns:

#### Pattern 1: Optimistic Updates
```dart
@riverpod
class UserList extends _$UserList {
  Future<void> toggleUserActive(String userId) async {
    final currentState = state.value;
    if (currentState == null) return;
    
    // Optimistic update
    final updatedItems = currentState.items.map((user) {
      if (user.id == userId) {
        return user.copyWith(isActive: !user.isActive);
      }
      return user;
    }).toList();
    
    state = AsyncValue.data(
      currentState.copyWith(items: updatedItems),
    );
    
    try {
      // Actual API call
      final service = ref.read(userServiceProvider);
      final user = await service.get(userId);
      await service.update(
        userId,
        CreateUpdateUserDto.fromUser(user, isActive: !user.isActive),
      );
    } catch (e) {
      // Revert on error
      await refresh();
      rethrow;
    }
  }
}
```

#### Pattern 2: Infinite Scroll
```dart
@riverpod
class InfiniteUserList extends _$InfiniteUserList {
  final List<UserDto> _allUsers = [];
  int _currentPage = 0;
  bool _hasMore = true;
  
  @override
  AsyncValue<List<UserDto>> build() {
    loadMore();
    return AsyncValue.data(_allUsers);
  }
  
  Future<void> loadMore() async {
    if (!_hasMore || state.isLoading) return;
    
    state = AsyncValue.loading();
    
    try {
      final service = ref.read(userServiceProvider);
      final result = await service.getList(
        PagedAndSortedResultRequestDto(
          skipCount: _currentPage * 20,
          maxResultCount: 20,
        ),
      );
      
      _allUsers.addAll(result.items);
      _currentPage++;
      _hasMore = _allUsers.length < result.totalCount;
      
      state = AsyncValue.data(_allUsers);
    } catch (e, stack) {
      state = AsyncValue.error(e, stack);
    }
  }
  
  Future<void> refresh() async {
    _allUsers.clear();
    _currentPage = 0;
    _hasMore = true;
    await loadMore();
  }
}
```

#### Pattern 3: Debounced Search
```dart
@riverpod
class DebouncedUserSearch extends _$DebouncedUserSearch {
  Timer? _debounceTimer;
  
  @override
  AsyncValue<List<UserDto>> build(String query) {
    if (query.isEmpty) {
      return const AsyncValue.data([]);
    }
    
    return const AsyncValue.loading();
  }
  
  void search(String query) {
    _debounceTimer?.cancel();
    
    if (query.isEmpty) {
      state = const AsyncValue.data([]);
      return;
    }
    
    state = const AsyncValue.loading();
    
    _debounceTimer = Timer(const Duration(milliseconds: 500), () {
      _performSearch(query);
    });
  }
  
  Future<void> _performSearch(String query) async {
    state = await AsyncValue.guard(() async {
      final service = ref.read(userServiceProvider);
      final result = await service.getList(
        PagedAndSortedResultRequestDto(
          filter: query,
          maxResultCount: 20,
        ),
      );
      return result.items;
    });
  }
}
```

### 6. ABP Error Handling

```dart
/// Extension for handling ABP exceptions
extension AbpErrorHandling on AsyncValue {
  String? get errorMessage {
    return when(
      data: (_) => null,
      loading: () => null,
      error: (error, _) {
        if (error is AbpException) {
          return error.message;
        } else if (error is DioException) {
          return _handleDioError(error);
        }
        return 'An unexpected error occurred';
      },
    );
  }
  
  Map<String, String> get validationErrors {
    return when(
      data: (_) => {},
      loading: () => {},
      error: (error, _) {
        if (error is AbpException && error.validationErrors != null) {
          final errors = <String, String>{};
          for (final validationError in error.validationErrors!) {
            final field = validationError.members.firstOrNull;
            if (field != null) {
              errors[field] = validationError.message;
            }
          }
          return errors;
        }
        return {};
      },
    );
  }
}

String _handleDioError(DioException error) {
  switch (error.type) {
    case DioExceptionType.connectionTimeout:
    case DioExceptionType.receiveTimeout:
    case DioExceptionType.sendTimeout:
      return 'Connection timeout. Please check your internet connection.';
    case DioExceptionType.badResponse:
      final statusCode = error.response?.statusCode;
      if (statusCode == 401) {
        return 'Unauthorized. Please login again.';
      } else if (statusCode == 403) {
        return 'You do not have permission to perform this action.';
      } else if (statusCode == 404) {
        return 'Resource not found.';
      }
      return 'Server error occurred.';
    case DioExceptionType.cancel:
      return 'Request cancelled.';
    default:
      return 'Network error occurred.';
  }
}
```

---

## Part 2: UI Component Documentation

[Continue with the previous UI documentation structure, but now integrate with the Riverpod providers]

### 1. Centralized Theme Management
[Same as before]

### 2. Atomic Widgets Documentation
[Same as before]

### 3. Composite Widgets Documentation
[Same as before]

### 4. Feature Widgets Documentation (with Riverpod Integration)

#### Example: User List Feature Widget

**Widget Name:** `UserListView`

**Purpose:** Displays paginated list of users with search, sort, and CRUD operations

**Riverpod Providers Used:**
- `userListProvider` - Main data source
- `userListCurrentPageProvider` - Current page number
- `userListTotalPagesProvider` - Total page count
- `userSearchQueryProvider` - Search state
- `deleteUser` - Delete operation

**ABP Integration:**
- Service: `IUserAppService`
- DTOs: `UserDto`, `PagedResultDto<UserDto>`
- Operations: GetList, Delete

**Code Example:**
```dart
class UserListView extends ConsumerWidget {
  const UserListView({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final usersAsync = ref.watch(userListProvider);
    final currentPage = ref.watch(userListCurrentPageProvider);
    final totalPages = ref.watch(userListTotalPagesProvider);

    return Scaffold(
      appBar: AppBar(
        title: const AppText('Users'),
        actions: [
          AppIconButton(
            icon: Icons.add,
            onPressed: () => _navigateToCreateUser(context),
          ),
        ],
      ),
      body: Column(
        children: [
          // Search bar
          Padding(
            padding: EdgeInsets.all(AppSpacing.md),
            child: AppSearchField(
              onChanged: (query) {
                ref.read(userListProvider.notifier).search(query);
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
                      onDelete: () => _confirmDelete(context, ref, user),
                    );
                  },
                );
              },
              loading: () => const AppLoadingIndicator(),
              error: (error, stack) => AppErrorWidget(
                error: error,
                onRetry: () => ref.invalidate(userListProvider),
              ),
            ),
          ),
          
          // Pagination
          if (usersAsync.hasValue && totalPages > 1)
            AppPaginationBar(
              currentPage: currentPage,
              totalPages: totalPages,
              onPreviousPage: () {
                ref.read(userListProvider.notifier).previousPage();
              },
              onNextPage: () {
                ref.read(userListProvider.notifier).nextPage();
              },
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
    UserDto user,
  ) async {
    final confirmed = await showDialog<bool>(
      context: context,
      builder: (context) => AppConfirmDialog(
        title: 'Delete User',
        message: 'Are you sure you want to delete ${user.userName}?',
      ),
    );

    if (confirmed == true) {
      try {
        await ref.read(deleteUserProvider(user.id).future);
        
        if (context.mounted) {
          AppSnackbar.showSuccess(
            context,
            'User deleted successfully',
          );
        }
      } catch (e) {
        if (context.mounted) {
          AppSnackbar.showError(
            context,
            'Failed to delete user: ${e.toString()}',
          );
        }
      }
    }
  }
}
```

#### Example: User Form Feature Widget

**Widget Name:** `UserFormWidget`

**Purpose:** Create or update user with validation and ABP error handling

**Riverpod Providers Used:**
- `userFormProvider` - Form state management
- `userProvider` (when editing) - Load existing user data
- `roleOptionsProvider` - Available roles for assignment

**ABP Integration:**
- Service: `IUserAppService`, `IRoleAppService`
- DTOs: `CreateUpdateUserDto`, `UserDto`, `RoleDto`
- Operations: Create, Update, GetList (roles)

**Code Example:**
```dart
class UserFormWidget extends ConsumerWidget {
  final String? userId; // null for create, id for update

  const UserFormWidget({super.key, this.userId});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final formState = ref.watch(userFormProvider(userId: userId));
    final rolesAsync = ref.watch(roleOptionsProvider);

    return Scaffold(
      appBar: AppBar(
        title: AppText(userId == null ? 'Create User' : 'Edit User'),
      ),
      body: SingleChildScrollView(
        padding: EdgeInsets.all(AppSpacing.lg),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            // User Name Field
            AppFormField(
              label: 'User Name',
              hint: 'Enter user name',
              initialValue: formState.userName,
              onChanged: (value) {
                ref.read(userFormProvider(userId: userId).notifier)
                   .updateUserName(value);
              },
              errorText: formState.errors['userName'],
              enabled: !formState.isSubmitting,
            ),
            
            SizedBox(height: AppSpacing.md),
            
            // Email Field
            AppFormField(
              label: 'Email',
              hint: 'Enter email address',
              keyboardType: TextInputType.emailAddress,
              initialValue: formState.email,
              onChanged: (value) {
                ref.read(userFormProvider(userId: userId).notifier)
                   .updateEmail(value);
              },
              errorText: formState.errors['email'],
              enabled: !formState.isSubmitting,
            ),
            
            SizedBox(height: AppSpacing.md),
            
            // Phone Number Field
            AppFormField(
              label: 'Phone Number',
              hint: 'Enter phone number (optional)',
              keyboardType: TextInputType.phone,
              initialValue: formState.phoneNumber,
              onChanged: (value) {
                ref.read(userFormProvider(userId: userId).notifier)
                   .updatePhoneNumber(value);
              },
              errorText: formState.errors['phoneNumber'],
              enabled: !formState.isSubmitting,
            ),
            
            SizedBox(height: AppSpacing.md),
            
            // Password Field (only for create or if changing password)
            if (userId == null)
              AppFormField(
                label: 'Password',
                hint: 'Enter password',
                obscureText: true,
                onChanged: (value) {
                  ref.read(userFormProvider(userId: userId).notifier)
                     .updatePassword(value);
                },
                errorText: formState.errors['password'],
                enabled: !formState.isSubmitting,
              ),
            
            if (userId == null) SizedBox(height: AppSpacing.md),
            
            // Is Active Switch
            AppSwitchTile(
              label: 'Active',
              value: formState.isActive,
              onChanged: formState.isSubmitting ? null : (value) {
                ref.read(userFormProvider(userId: userId).notifier)
                   .updateIsActive(value);
              },
            ),
            
            SizedBox(height: AppSpacing.md),
            
            // Roles Multi-Select
            AppText('Roles', style: AppTextStyles.label),
            SizedBox(height: AppSpacing.xs),
            rolesAsync.when(
              data: (roles) => AppMultiSelectChips(
                options: roles.map((r) => r.name).toList(),
                selectedOptions: formState.roleNames,
                onChanged: formState.isSubmitting ? null : (selected) {
                  ref.read(userFormProvider(userId: userId).notifier)
                     .updateRoleNames(selected);
                },
              ),
              loading: () => const AppLoadingIndicator(),
              error: (error, _) => AppErrorText(error.toString()),
            ),
            
            // Submission Error
            if (formState.submitError != null) ...[
              SizedBox(height: AppSpacing.md),
              AppErrorBanner(message: formState.submitError!),
            ],
            
            SizedBox(height: AppSpacing.xl),
            
            // Submit Button
            AppButton(
              label: userId == null ? 'Create User' : 'Update User',
              onPressed: formState.isSubmitting ? null : () async {
                final result = await ref
                    .read(userFormProvider(userId: userId).notifier)
                    .submit();
                
                if (result != null && context.mounted) {
                  AppSnackbar.showSuccess(
                    context,
                    userId == null 
                        ? 'User created successfully'
                        : 'User updated successfully',
                  );
                  Navigator.of(context).pop(result);
                }
              },
              isLoading: formState.isSubmitting,
            ),
          ],
        ),
      ),
    );
  }
}
```

---

## Part 3: Complete Documentation Structure

```
📁 Documentation Root
│
├── 📄 1. Introduction
│   ├── Architecture Overview
│   ├── ABP Framework Integration
│   ├── Riverpod State Management
│   └── Design Principles
│
├── 📄 2. ABP Application Layer
│   ├── Application Services
│   │   ├── IUserAppService
│   │   ├── IRoleAppService
│   │   └── ITenantAppService
│   │
│   └── DTOs
│       ├── Output DTOs (UserDto, RoleDto, etc.)
│       ├── Input DTOs (CreateUpdateUserDto, etc.)
│       └── Paged DTOs (PagedResultDto, etc.)
│
├── 📄 3. Riverpod State Management
│   ├── Provider Architecture
│   │   ├── Service Providers
│   │   ├── List Providers
│   │   ├── Entity Providers (Families)
│   │   ├── Form Providers
│   │   ├── Delete Operation Providers
│   │   └── Lookup/Reference Providers
│   │
│   ├── State Models
│   │   ├── Form States
│   │   ├── List States
│   │   └── UI States
│   │
│   ├── State Management Patterns
│   │   ├── Optimistic Updates
│   │   ├── Infinite Scroll
│   │   ├── Debounced Search
│   │   ├── Cache Invalidation
│   │   └── Error Recovery
│   │
│   └── ABP Error Handling
│       ├── Exception Mapping
│       ├── Validation Error Display
│       └── Network Error Handling
│
├── 📄 4. Theme System
│   ├── Colors
│   ├── Typography
│   ├── Spacing
│   └── Sizing
│
├── 📄 5. Atomic Widgets
│   ├── Buttons
│   ├── Text Inputs
│   ├── Selection Controls
│   ├── Feedback Components
│   └── Typography Components
│
├── 📄 6. Composite Widgets
│   ├── Form Components
│   ├── List Components
│   ├── Data Display Components
│   └── Navigation Components
│
├── 📄 7. Feature Widgets
│   ├── User Management
│   │   ├── UserListView (with provider integration)
│   │   ├── UserFormWidget (with provider integration)
│   │   └── UserDetailsView (with provider integration)
│   │
│   ├── Role Management
│   │   ├── RoleListView
│   │   ├── RoleFormWidget
│   │   └── RolePermissionsWidget
│   │
│   └── [Other Features]
│
├── 📄 8. Integration Patterns
│   ├── Widget-Provider Integration
│   ├── Navigation with State
│   ├── Form Submission Flow
│   └── List Operations (CRUD)
│
├── 📄 9. Best Practices
│   ├── File Structure
│   ├── Naming Conventions
│   ├── Code Organization
│   ├── Testing Strategies
│   └── Performance Optimization
│
└── 📄 10. Appendix
    ├── Provider Dependency Graph
    ├── Common Patterns
    ├── Troubleshooting Guide
    └── Migration Guide
```

---

## Output Request

Please provide the following information about your ABP application:

1. **ABP Application Services:**
   - List of application service interfaces (e.g., IUserAppService, IProductAppService)
   - Methods available in each service
   - Any custom methods beyond standard CRUD

2. **ABP DTOs:**
   - Output DTOs with all properties and types
   - Input DTOs with validation rules
   - Any nested or complex DTOs

3. **ABP Entities:**
   - Entity names and their relationships
   - Key properties and their types
   - Navigation properties

4. **Business Rules:**
   - Permission requirements
   - Validation rules
   - Business logic constraints

5. **UI Requirements:**
   - List of screens/features needed
   - User workflows
   - Special UI behaviors

Based on this information, I will generate:
- ✅ Complete Riverpod provider documentation for each ABP service
- ✅ State models and form states for all DTOs
- ✅ UI component hierarchy (Atomic → Composite → Feature)
- ✅ Integration examples showing widgets using providers
- ✅ Error handling and validation patterns
- ✅ Complete, working code examples

Example format for your input:
```
Service: IUserAppService
Methods: GetListAsync, GetAsync, CreateAsync, UpdateAsync, DeleteAsync
DTOs:
  - UserDto: { id, userName, email, phoneNumber, isActive, ... }
  - CreateUpdateUserDto: { userName, email, password, ... }
Relationships: User has many Roles
Permissions: Users.Create, Users.Edit, Users.Delete
```
