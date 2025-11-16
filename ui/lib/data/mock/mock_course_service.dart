import '../models/course_dto.dart';
import '../models/paged_result_dto.dart';

/// Mock course service
class MockCourseService {
  static final List<CourseDto> _courses = _generateMockCourses();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 400));
  }

  /// Get paginated courses
  static Future<PagedResultDto<CourseDto>> getList({
    int skipCount = 0,
    int maxResultCount = 10,
    String? filter,
    CourseCategory? category,
    CourseDifficulty? difficulty,
  }) async {
    await _simulateDelay();

    var filtered = List<CourseDto>.from(_courses);

    // Apply filters
    if (filter != null && filter.isNotEmpty) {
      filtered = filtered.where((course) {
        return course.title.toLowerCase().contains(filter.toLowerCase()) ||
            course.description.toLowerCase().contains(filter.toLowerCase());
      }).toList();
    }

    if (category != null) {
      filtered = filtered.where((course) => course.category == category).toList();
    }

    if (difficulty != null) {
      filtered = filtered.where((course) => course.difficulty == difficulty).toList();
    }

    // Apply pagination
    final paginatedItems = filtered.skip(skipCount).take(maxResultCount).toList();

    return PagedResultDto(
      items: paginatedItems,
      totalCount: filtered.length,
    );
  }

  /// Get single course
  static Future<CourseDto> get(String id) async {
    await _simulateDelay();

    try {
      return _courses.firstWhere((course) => course.id == id);
    } catch (e) {
      throw Exception('Course not found');
    }
  }

  /// Get featured courses
  static Future<List<CourseDto>> getFeatured() async {
    await _simulateDelay();
    return _courses.where((course) => course.isFeatured).take(5).toList();
  }

  /// Create course
  static Future<CourseDto> create(CreateUpdateCourseDto input) async {
    await _simulateDelay();

    final newCourse = CourseDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      title: input.title,
      description: input.description,
      thumbnail: input.thumbnail,
      instructorId: input.instructorId,
      instructorName: 'Instructor Name',
      category: input.category,
      difficulty: input.difficulty,
      status: CourseStatus.draft,
      tags: input.tags,
      prerequisites: input.prerequisites,
      estimatedDuration: input.estimatedDuration,
      lessonCount: 0,
      isFeatured: input.isFeatured,
      isCertified: input.isCertified,
      price: input.price,
      isFree: input.isFree,
      creationTime: DateTime.now(),
    );

    _courses.insert(0, newCourse);
    return newCourse;
  }

  /// Update course
  static Future<CourseDto> update(String id, CreateUpdateCourseDto input) async {
    await _simulateDelay();

    final index = _courses.indexWhere((course) => course.id == id);
    if (index == -1) {
      throw Exception('Course not found');
    }

    final updatedCourse = _courses[index].copyWith(
      title: input.title,
      description: input.description,
      thumbnail: input.thumbnail,
      category: input.category,
      difficulty: input.difficulty,
      tags: input.tags,
      prerequisites: input.prerequisites,
      estimatedDuration: input.estimatedDuration,
      isFeatured: input.isFeatured,
      isCertified: input.isCertified,
      price: input.price,
      isFree: input.isFree,
      lastModificationTime: DateTime.now(),
    );

    _courses[index] = updatedCourse;
    return updatedCourse;
  }

  /// Delete course
  static Future<void> delete(String id) async {
    await _simulateDelay();
    final initialLength = _courses.length;
    _courses.removeWhere((course) => course.id == id);
    if (_courses.length == initialLength) {
      throw Exception('Course not found');
    }
  }

  /// Generate mock courses
  static List<CourseDto> _generateMockCourses() {
    final now = DateTime.now();
    return [
      CourseDto(
        id: 'course-1',
        title: 'Flutter Advanced Masterclass',
        description:
            'Master advanced Flutter concepts including state management, animations, custom widgets, and performance optimization.',
        thumbnail: 'https://picsum.photos/seed/flutter/400/300',
        instructorId: 'instructor-1',
        instructorName: 'Sarah Johnson',
        category: CourseCategory.mobile,
        difficulty: CourseDifficulty.advanced,
        status: CourseStatus.published,
        tags: ['Flutter', 'Dart', 'Mobile', 'State Management'],
        prerequisites: ['Basic Flutter knowledge', 'Dart fundamentals'],
        estimatedDuration: 2400,
        lessonCount: 48,
        enrollmentCount: 1250,
        rating: 4.8,
        reviewCount: 342,
        isFeatured: true,
        isCertified: true,
        price: 99.99,
        creationTime: now.subtract(const Duration(days: 180)),
        publishedDate: now.subtract(const Duration(days: 170)),
      ),
      CourseDto(
        id: 'course-2',
        title: 'React Native Complete Guide',
        description:
            'Build cross-platform mobile apps with React Native. Learn from basics to advanced topics.',
        thumbnail: 'https://picsum.photos/seed/react/400/300',
        instructorId: 'instructor-2',
        instructorName: 'Michael Chen',
        category: CourseCategory.mobile,
        difficulty: CourseDifficulty.intermediate,
        status: CourseStatus.published,
        tags: ['React Native', 'JavaScript', 'Mobile'],
        prerequisites: ['JavaScript basics', 'React fundamentals'],
        estimatedDuration: 1800,
        lessonCount: 36,
        enrollmentCount: 890,
        rating: 4.6,
        reviewCount: 215,
        isFeatured: true,
        isCertified: true,
        price: 79.99,
        creationTime: now.subtract(const Duration(days: 150)),
        publishedDate: now.subtract(const Duration(days: 140)),
      ),
      CourseDto(
        id: 'course-3',
        title: 'Python for Data Science',
        description:
            'Learn Python programming and data analysis with pandas, NumPy, and visualization libraries.',
        thumbnail: 'https://picsum.photos/seed/python/400/300',
        instructorId: 'instructor-3',
        instructorName: 'Dr. Emily Watson',
        category: CourseCategory.dataScience,
        difficulty: CourseDifficulty.beginner,
        status: CourseStatus.published,
        tags: ['Python', 'Data Science', 'Pandas', 'NumPy'],
        prerequisites: [],
        estimatedDuration: 1500,
        lessonCount: 30,
        enrollmentCount: 2150,
        rating: 4.9,
        reviewCount: 567,
        isFeatured: true,
        isCertified: true,
        isFree: true,
        creationTime: now.subtract(const Duration(days: 200)),
        publishedDate: now.subtract(const Duration(days: 195)),
      ),
      CourseDto(
        id: 'course-4',
        title: 'UI/UX Design Principles',
        description:
            'Master user interface and user experience design principles. Create beautiful and functional designs.',
        thumbnail: 'https://picsum.photos/seed/design/400/300',
        instructorId: 'instructor-4',
        instructorName: 'Alex Rivera',
        category: CourseCategory.design,
        difficulty: CourseDifficulty.intermediate,
        status: CourseStatus.published,
        tags: ['UI', 'UX', 'Design', 'Figma'],
        prerequisites: ['Basic design knowledge'],
        estimatedDuration: 1200,
        lessonCount: 24,
        enrollmentCount: 1680,
        rating: 4.7,
        reviewCount: 423,
        isFeatured: true,
        isCertified: true,
        price: 89.99,
        creationTime: now.subtract(const Duration(days: 120)),
        publishedDate: now.subtract(const Duration(days: 110)),
      ),
      CourseDto(
        id: 'course-5',
        title: 'Docker & Kubernetes Essentials',
        description:
            'Learn containerization and orchestration with Docker and Kubernetes for modern DevOps.',
        thumbnail: 'https://picsum.photos/seed/docker/400/300',
        instructorId: 'instructor-5',
        instructorName: 'David Kumar',
        category: CourseCategory.devOps,
        difficulty: CourseDifficulty.advanced,
        status: CourseStatus.published,
        tags: ['Docker', 'Kubernetes', 'DevOps', 'Containers'],
        prerequisites: ['Linux basics', 'Command line knowledge'],
        estimatedDuration: 2000,
        lessonCount: 40,
        enrollmentCount: 745,
        rating: 4.5,
        reviewCount: 189,
        isFeatured: false,
        isCertified: true,
        price: 119.99,
        creationTime: now.subtract(const Duration(days: 90)),
        publishedDate: now.subtract(const Duration(days: 85)),
      ),
      CourseDto(
        id: 'course-6',
        title: 'JavaScript ES6+ Modern Features',
        description:
            'Explore modern JavaScript features including async/await, promises, modules, and more.',
        thumbnail: 'https://picsum.photos/seed/javascript/400/300',
        instructorId: 'instructor-1',
        instructorName: 'Sarah Johnson',
        category: CourseCategory.programming,
        difficulty: CourseDifficulty.intermediate,
        status: CourseStatus.published,
        tags: ['JavaScript', 'ES6', 'Web Development'],
        prerequisites: ['Basic JavaScript'],
        estimatedDuration: 900,
        lessonCount: 18,
        enrollmentCount: 1920,
        rating: 4.8,
        reviewCount: 512,
        isFeatured: false,
        isCertified: false,
        isFree: true,
        creationTime: now.subtract(const Duration(days: 60)),
        publishedDate: now.subtract(const Duration(days: 55)),
      ),
      CourseDto(
        id: 'course-7',
        title: 'Digital Marketing Fundamentals',
        description:
            'Learn the fundamentals of digital marketing including SEO, social media, and content marketing.',
        thumbnail: 'https://picsum.photos/seed/marketing/400/300',
        instructorId: 'instructor-6',
        instructorName: 'Jennifer Lee',
        category: CourseCategory.marketing,
        difficulty: CourseDifficulty.beginner,
        status: CourseStatus.published,
        tags: ['Marketing', 'SEO', 'Social Media'],
        prerequisites: [],
        estimatedDuration: 1100,
        lessonCount: 22,
        enrollmentCount: 2340,
        rating: 4.6,
        reviewCount: 678,
        isFeatured: false,
        isCertified: true,
        price: 69.99,
        creationTime: now.subtract(const Duration(days: 100)),
        publishedDate: now.subtract(const Duration(days: 95)),
      ),
      CourseDto(
        id: 'course-8',
        title: 'Full Stack Web Development',
        description:
            'Become a full-stack developer with this comprehensive course covering front-end and back-end technologies.',
        thumbnail: 'https://picsum.photos/seed/fullstack/400/300',
        instructorId: 'instructor-2',
        instructorName: 'Michael Chen',
        category: CourseCategory.web,
        difficulty: CourseDifficulty.advanced,
        status: CourseStatus.published,
        tags: ['Web Development', 'Full Stack', 'Node.js', 'React'],
        prerequisites: ['HTML', 'CSS', 'JavaScript'],
        estimatedDuration: 3000,
        lessonCount: 60,
        enrollmentCount: 1540,
        rating: 4.9,
        reviewCount: 421,
        isFeatured: true,
        isCertified: true,
        price: 149.99,
        creationTime: now.subtract(const Duration(days: 220)),
        publishedDate: now.subtract(const Duration(days: 210)),
      ),
    ];
  }
}
