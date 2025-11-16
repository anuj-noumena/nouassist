// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'gtd_review_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(reviewList)
const reviewListProvider = ReviewListFamily._();

final class ReviewListProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<GtdReviewDto>>,
          List<GtdReviewDto>,
          FutureOr<List<GtdReviewDto>>
        >
    with
        $FutureModifier<List<GtdReviewDto>>,
        $FutureProvider<List<GtdReviewDto>> {
  const ReviewListProvider._({
    required ReviewListFamily super.from,
    required (String, {ReviewType? reviewType, bool? isCompleted})
    super.argument,
  }) : super(
         retry: null,
         name: r'reviewListProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$reviewListHash();

  @override
  String toString() {
    return r'reviewListProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<List<GtdReviewDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<GtdReviewDto>> create(Ref ref) {
    final argument =
        this.argument as (String, {ReviewType? reviewType, bool? isCompleted});
    return reviewList(
      ref,
      argument.$1,
      reviewType: argument.reviewType,
      isCompleted: argument.isCompleted,
    );
  }

  @override
  bool operator ==(Object other) {
    return other is ReviewListProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$reviewListHash() => r'6278d3caa9f4a285f2c2444d29c0f511ee61c4b8';

final class ReviewListFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<List<GtdReviewDto>>,
          (String, {ReviewType? reviewType, bool? isCompleted})
        > {
  const ReviewListFamily._()
    : super(
        retry: null,
        name: r'reviewListProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  ReviewListProvider call(
    String userId, {
    ReviewType? reviewType,
    bool? isCompleted,
  }) => ReviewListProvider._(
    argument: (userId, reviewType: reviewType, isCompleted: isCompleted),
    from: this,
  );

  @override
  String toString() => r'reviewListProvider';
}

@ProviderFor(review)
const reviewProvider = ReviewFamily._();

final class ReviewProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdReviewDto>,
          GtdReviewDto,
          FutureOr<GtdReviewDto>
        >
    with $FutureModifier<GtdReviewDto>, $FutureProvider<GtdReviewDto> {
  const ReviewProvider._({
    required ReviewFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'reviewProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$reviewHash();

  @override
  String toString() {
    return r'reviewProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<GtdReviewDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdReviewDto> create(Ref ref) {
    final argument = this.argument as String;
    return review(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is ReviewProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$reviewHash() => r'5cfc883c030c8cbbe8447f4c685bf8dbd375a3e8';

final class ReviewFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<GtdReviewDto>, String> {
  const ReviewFamily._()
    : super(
        retry: null,
        name: r'reviewProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  ReviewProvider call(String id) => ReviewProvider._(argument: id, from: this);

  @override
  String toString() => r'reviewProvider';
}

@ProviderFor(createReview)
const createReviewProvider = CreateReviewFamily._();

final class CreateReviewProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdReviewDto>,
          GtdReviewDto,
          FutureOr<GtdReviewDto>
        >
    with $FutureModifier<GtdReviewDto>, $FutureProvider<GtdReviewDto> {
  const CreateReviewProvider._({
    required CreateReviewFamily super.from,
    required ({String userId, ReviewType reviewType}) super.argument,
  }) : super(
         retry: null,
         name: r'createReviewProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$createReviewHash();

  @override
  String toString() {
    return r'createReviewProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<GtdReviewDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdReviewDto> create(Ref ref) {
    final argument = this.argument as ({String userId, ReviewType reviewType});
    return createReview(
      ref,
      userId: argument.userId,
      reviewType: argument.reviewType,
    );
  }

  @override
  bool operator ==(Object other) {
    return other is CreateReviewProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$createReviewHash() => r'ead54769a6af34b65e7bb61ce8f978954210d17a';

final class CreateReviewFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<GtdReviewDto>,
          ({String userId, ReviewType reviewType})
        > {
  const CreateReviewFamily._()
    : super(
        retry: null,
        name: r'createReviewProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CreateReviewProvider call({
    required String userId,
    required ReviewType reviewType,
  }) => CreateReviewProvider._(
    argument: (userId: userId, reviewType: reviewType),
    from: this,
  );

  @override
  String toString() => r'createReviewProvider';
}

@ProviderFor(updateReviewChecklist)
const updateReviewChecklistProvider = UpdateReviewChecklistFamily._();

final class UpdateReviewChecklistProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdReviewDto>,
          GtdReviewDto,
          FutureOr<GtdReviewDto>
        >
    with $FutureModifier<GtdReviewDto>, $FutureProvider<GtdReviewDto> {
  const UpdateReviewChecklistProvider._({
    required UpdateReviewChecklistFamily super.from,
    required (String, List<ReviewChecklistItemDto>) super.argument,
  }) : super(
         retry: null,
         name: r'updateReviewChecklistProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$updateReviewChecklistHash();

  @override
  String toString() {
    return r'updateReviewChecklistProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<GtdReviewDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdReviewDto> create(Ref ref) {
    final argument = this.argument as (String, List<ReviewChecklistItemDto>);
    return updateReviewChecklist(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is UpdateReviewChecklistProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$updateReviewChecklistHash() =>
    r'194edc5f09b68bf534fc36c774301e4f65096697';

final class UpdateReviewChecklistFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<GtdReviewDto>,
          (String, List<ReviewChecklistItemDto>)
        > {
  const UpdateReviewChecklistFamily._()
    : super(
        retry: null,
        name: r'updateReviewChecklistProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  UpdateReviewChecklistProvider call(
    String id,
    List<ReviewChecklistItemDto> checklist,
  ) => UpdateReviewChecklistProvider._(argument: (id, checklist), from: this);

  @override
  String toString() => r'updateReviewChecklistProvider';
}

@ProviderFor(completeReview)
const completeReviewProvider = CompleteReviewFamily._();

final class CompleteReviewProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdReviewDto>,
          GtdReviewDto,
          FutureOr<GtdReviewDto>
        >
    with $FutureModifier<GtdReviewDto>, $FutureProvider<GtdReviewDto> {
  const CompleteReviewProvider._({
    required CompleteReviewFamily super.from,
    required (String, String?) super.argument,
  }) : super(
         retry: null,
         name: r'completeReviewProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$completeReviewHash();

  @override
  String toString() {
    return r'completeReviewProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<GtdReviewDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdReviewDto> create(Ref ref) {
    final argument = this.argument as (String, String?);
    return completeReview(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is CompleteReviewProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$completeReviewHash() => r'183444f8ec90f1c7ba05dfae3042e94fabb33859';

final class CompleteReviewFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<GtdReviewDto>, (String, String?)> {
  const CompleteReviewFamily._()
    : super(
        retry: null,
        name: r'completeReviewProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CompleteReviewProvider call(String id, String? notes) =>
      CompleteReviewProvider._(argument: (id, notes), from: this);

  @override
  String toString() => r'completeReviewProvider';
}
