// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'quiz_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(quiz)
const quizProvider = QuizFamily._();

final class QuizProvider
    extends $FunctionalProvider<AsyncValue<QuizDto>, QuizDto, FutureOr<QuizDto>>
    with $FutureModifier<QuizDto>, $FutureProvider<QuizDto> {
  const QuizProvider._({
    required QuizFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'quizProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$quizHash();

  @override
  String toString() {
    return r'quizProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<QuizDto> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<QuizDto> create(Ref ref) {
    final argument = this.argument as String;
    return quiz(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is QuizProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$quizHash() => r'afe2aa2311c8e16c1aecd98af57a184014765692';

final class QuizFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<QuizDto>, String> {
  const QuizFamily._()
    : super(
        retry: null,
        name: r'quizProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  QuizProvider call(String id) => QuizProvider._(argument: id, from: this);

  @override
  String toString() => r'quizProvider';
}

@ProviderFor(courseQuizzes)
const courseQuizzesProvider = CourseQuizzesFamily._();

final class CourseQuizzesProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<QuizDto>>,
          List<QuizDto>,
          FutureOr<List<QuizDto>>
        >
    with $FutureModifier<List<QuizDto>>, $FutureProvider<List<QuizDto>> {
  const CourseQuizzesProvider._({
    required CourseQuizzesFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'courseQuizzesProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$courseQuizzesHash();

  @override
  String toString() {
    return r'courseQuizzesProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<List<QuizDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<QuizDto>> create(Ref ref) {
    final argument = this.argument as String;
    return courseQuizzes(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CourseQuizzesProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$courseQuizzesHash() => r'e18c51150ac7aa751d9c5e49dc94ef1741d319f9';

final class CourseQuizzesFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<List<QuizDto>>, String> {
  const CourseQuizzesFamily._()
    : super(
        retry: null,
        name: r'courseQuizzesProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CourseQuizzesProvider call(String courseId) =>
      CourseQuizzesProvider._(argument: courseId, from: this);

  @override
  String toString() => r'courseQuizzesProvider';
}

@ProviderFor(submitQuiz)
const submitQuizProvider = SubmitQuizFamily._();

final class SubmitQuizProvider
    extends
        $FunctionalProvider<
          AsyncValue<QuizAttemptDto>,
          QuizAttemptDto,
          FutureOr<QuizAttemptDto>
        >
    with $FutureModifier<QuizAttemptDto>, $FutureProvider<QuizAttemptDto> {
  const SubmitQuizProvider._({
    required SubmitQuizFamily super.from,
    required ({String userId, String quizId, Map<String, String> answers})
    super.argument,
  }) : super(
         retry: null,
         name: r'submitQuizProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$submitQuizHash();

  @override
  String toString() {
    return r'submitQuizProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<QuizAttemptDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<QuizAttemptDto> create(Ref ref) {
    final argument =
        this.argument
            as ({String userId, String quizId, Map<String, String> answers});
    return submitQuiz(
      ref,
      userId: argument.userId,
      quizId: argument.quizId,
      answers: argument.answers,
    );
  }

  @override
  bool operator ==(Object other) {
    return other is SubmitQuizProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$submitQuizHash() => r'4b668bbddbbc2ad74e70225dd64cbea2f9dfb17f';

final class SubmitQuizFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<QuizAttemptDto>,
          ({String userId, String quizId, Map<String, String> answers})
        > {
  const SubmitQuizFamily._()
    : super(
        retry: null,
        name: r'submitQuizProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  SubmitQuizProvider call({
    required String userId,
    required String quizId,
    required Map<String, String> answers,
  }) => SubmitQuizProvider._(
    argument: (userId: userId, quizId: quizId, answers: answers),
    from: this,
  );

  @override
  String toString() => r'submitQuizProvider';
}

@ProviderFor(quizAttempts)
const quizAttemptsProvider = QuizAttemptsFamily._();

final class QuizAttemptsProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<QuizAttemptDto>>,
          List<QuizAttemptDto>,
          FutureOr<List<QuizAttemptDto>>
        >
    with
        $FutureModifier<List<QuizAttemptDto>>,
        $FutureProvider<List<QuizAttemptDto>> {
  const QuizAttemptsProvider._({
    required QuizAttemptsFamily super.from,
    required (String, {String? quizId}) super.argument,
  }) : super(
         retry: null,
         name: r'quizAttemptsProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$quizAttemptsHash();

  @override
  String toString() {
    return r'quizAttemptsProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<List<QuizAttemptDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<QuizAttemptDto>> create(Ref ref) {
    final argument = this.argument as (String, {String? quizId});
    return quizAttempts(ref, argument.$1, quizId: argument.quizId);
  }

  @override
  bool operator ==(Object other) {
    return other is QuizAttemptsProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$quizAttemptsHash() => r'95d69c26466ba63bd6d336265dddc1d3f335a40d';

final class QuizAttemptsFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<List<QuizAttemptDto>>,
          (String, {String? quizId})
        > {
  const QuizAttemptsFamily._()
    : super(
        retry: null,
        name: r'quizAttemptsProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  QuizAttemptsProvider call(String userId, {String? quizId}) =>
      QuizAttemptsProvider._(argument: (userId, quizId: quizId), from: this);

  @override
  String toString() => r'quizAttemptsProvider';
}
