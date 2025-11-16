import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/course_dto.dart';
import '../data/models/paged_result_dto.dart';
import '../data/mock/mock_course_service.dart';

part 'course_provider.g.dart';

@riverpod
class CourseList extends _$CourseList {
  int _skipCount = 0;
  final int _maxResultCount = 10;
  String? _filter;
  CourseCategory? _categoryFilter;
  CourseDifficulty? _difficultyFilter;

  @override
  Future<PagedResultDto<CourseDto>> build() async {
    return _loadCourses();
  }

  Future<PagedResultDto<CourseDto>> _loadCourses() async {
    return await MockCourseService.getList(
      skipCount: _skipCount,
      maxResultCount: _maxResultCount,
      filter: _filter,
      category: _categoryFilter,
      difficulty: _difficultyFilter,
    );
  }

  Future<void> search(String? query) async {
    _filter = query;
    _skipCount = 0;
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadCourses());
  }

  Future<void> setCategory(CourseCategory? category) async {
    _categoryFilter = category;
    _skipCount = 0;
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadCourses());
  }

  Future<void> setDifficulty(CourseDifficulty? difficulty) async {
    _difficultyFilter = difficulty;
    _skipCount = 0;
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadCourses());
  }

  Future<void> nextPage() async {
    final currentData = state.value;
    if (currentData == null) return;

    if (_skipCount + _maxResultCount < currentData.totalCount) {
      _skipCount += _maxResultCount;
      state = const AsyncValue.loading();
      state = await AsyncValue.guard(() => _loadCourses());
    }
  }

  Future<void> previousPage() async {
    if (_skipCount > 0) {
      _skipCount = (_skipCount - _maxResultCount).clamp(0, double.infinity).toInt();
      state = const AsyncValue.loading();
      state = await AsyncValue.guard(() => _loadCourses());
    }
  }

  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadCourses());
  }
}

@riverpod
Future<CourseDto> course(Ref ref, String id) async {
  return await MockCourseService.get(id);
}

@riverpod
Future<List<CourseDto>> featuredCourses(Ref ref) async {
  return await MockCourseService.getFeatured();
}

@riverpod
Future<void> createCourse(Ref ref,
  CreateUpdateCourseDto input,
) async {
  await MockCourseService.create(input);
  ref.invalidate(courseListProvider);
}

@riverpod
Future<void> deleteCourse(Ref ref, String id) async {
  await MockCourseService.delete(id);
  ref.invalidate(courseListProvider);
}
