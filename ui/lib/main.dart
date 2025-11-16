import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:ui/widgets/feature/gtd/contexts_screen.dart';
import 'package:ui/widgets/feature/gtd/gtd_dashboard.dart';
import 'package:ui/widgets/feature/gtd/inbox_screen.dart';
import 'package:ui/widgets/feature/gtd/next_actions_screen.dart';
import 'package:ui/widgets/feature/gtd/someday_maybe_screen.dart';
import 'package:ui/widgets/feature/gtd/waiting_for_screen.dart';
import 'package:ui/widgets/feature/gtd/weekly_review_screen.dart';
import 'core/theme/app_theme.dart';
import 'widgets/feature/task/task_list_view.dart';
import 'widgets/feature/task/task_board_view.dart';
import 'widgets/feature/project/project_dashboard.dart';
import 'widgets/feature/scheduling/calendar_screen.dart';
import 'widgets/feature/scheduling/time_block_screen.dart';
import 'widgets/feature/scheduling/working_schedule_screen.dart';
import 'widgets/feature/learning/learning_dashboard.dart';
import 'widgets/feature/learning/course_list_screen.dart';
import 'widgets/feature/learning/certificates_screen.dart';

void main() {
  runApp(
    const ProviderScope(
      child: MyApp(),
    ),
  );
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Complete Management System',
      theme: AppTheme.lightTheme,
      debugShowCheckedModeBanner: false,
      home: const HomeScreen(),
    );
  }
}

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  int _selectedIndex = 0;

  final List<Widget> _screens = [
    const ProjectDashboard(),
    const TaskListView(),
    const TaskBoardView(),
    const CalendarScreen(),
    const GtdDashboard(),
    const LearningDashboard(),
  ];

  @override
  Widget build(BuildContext context) {
    return LayoutBuilder(
      builder: (context, constraints) {
        final bool isLargeScreen = constraints.maxWidth >= 600;

        return Scaffold(
          body: Row(
            children: [
              if (isLargeScreen)
                NavigationRail(
                  extended: constraints.maxWidth >= 900,
                  selectedIndex: _selectedIndex,
                  onDestinationSelected: (index) {
                    setState(() {
                      _selectedIndex = index;
                    });
                  },
                  labelType: constraints.maxWidth >= 900
                      ? NavigationRailLabelType.none
                      : NavigationRailLabelType.all,
                  destinations: const [
                    NavigationRailDestination(
                      icon: Icon(Icons.dashboard_outlined),
                      selectedIcon: Icon(Icons.dashboard),
                      label: Text('Dashboard'),
                    ),
                    NavigationRailDestination(
                      icon: Icon(Icons.task_outlined),
                      selectedIcon: Icon(Icons.task),
                      label: Text('Tasks'),
                    ),
                    NavigationRailDestination(
                      icon: Icon(Icons.view_column_outlined),
                      selectedIcon: Icon(Icons.view_column),
                      label: Text('Board'),
                    ),
                    NavigationRailDestination(
                      icon: Icon(Icons.calendar_today_outlined),
                      selectedIcon: Icon(Icons.calendar_today),
                      label: Text('Calendar'),
                    ),
                    NavigationRailDestination(
                      icon: Icon(Icons.check_circle_outline),
                      selectedIcon: Icon(Icons.check_circle),
                      label: Text('GTD'),
                    ),
                    NavigationRailDestination(
                      icon: Icon(Icons.school_outlined),
                      selectedIcon: Icon(Icons.school),
                      label: Text('Learning'),
                    ),
                  ],
                  leading: isLargeScreen
                      ? Padding(
                          padding: const EdgeInsets.symmetric(vertical: 8.0),
                          child: FloatingActionButton(
                            elevation: 0,
                            onPressed: () {
                              Scaffold.of(context).openDrawer();
                            },
                            child: const Icon(Icons.menu),
                          ),
                        )
                      : null,
                ),
              const VerticalDivider(thickness: 1, width: 1),
              Expanded(
                child: _screens[_selectedIndex],
              ),
            ],
          ),
          bottomNavigationBar: isLargeScreen
              ? null
              : NavigationBar(
                  selectedIndex: _selectedIndex,
                  onDestinationSelected: (index) {
                    setState(() {
                      _selectedIndex = index;
                    });
                  },
                  destinations: const [
                    NavigationDestination(
                      icon: Icon(Icons.dashboard_outlined),
                      selectedIcon: Icon(Icons.dashboard),
                      label: 'Dashboard',
                    ),
                    NavigationDestination(
                      icon: Icon(Icons.task_outlined),
                      selectedIcon: Icon(Icons.task),
                      label: 'Tasks',
                    ),
                    NavigationDestination(
                      icon: Icon(Icons.view_column_outlined),
                      selectedIcon: Icon(Icons.view_column),
                      label: 'Board',
                    ),
                    NavigationDestination(
                      icon: Icon(Icons.calendar_today_outlined),
                      selectedIcon: Icon(Icons.calendar_today),
                      label: 'Calendar',
                    ),
                    NavigationDestination(
                      icon: Icon(Icons.check_circle_outline),
                      selectedIcon: Icon(Icons.check_circle),
                      label: 'GTD',
                    ),
                    NavigationDestination(
                      icon: Icon(Icons.school_outlined),
                      selectedIcon: Icon(Icons.school),
                      label: 'Learning',
                    ),
                  ],
                ),
          drawer: Drawer(
        child: ListView(
          children: [
            const DrawerHeader(
              decoration: BoxDecoration(
                gradient: LinearGradient(
                  colors: [Color(0xFF2196F3), Color(0xFF1976D2)],
                ),
              ),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                mainAxisAlignment: MainAxisAlignment.end,
                children: [
                  CircleAvatar(
                    radius: 35,
                    backgroundColor: Colors.white,
                    child: Icon(
                      Icons.person,
                      size: 45,
                      color: Color(0xFF2196F3),
                    ),
                  ),
                  SizedBox(height: 12),
                  Text(
                    'John Doe',
                    style: TextStyle(
                      color: Colors.white,
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  Text(
                    'john.doe@example.com',
                    style: TextStyle(
                      color: Colors.white70,
                      fontSize: 13,
                    ),
                  ),
                ],
              ),
            ),
            ListTile(
              leading: const Icon(Icons.dashboard),
              title: const Text('Dashboard'),
              onTap: () {
                Navigator.pop(context);
                setState(() => _selectedIndex = 0);
              },
            ),
            const Padding(
              padding: EdgeInsets.all(16.0),
              child: Text(
                'GTD',
                style: TextStyle(
                  fontSize: 12,
                  fontWeight: FontWeight.bold,
                  color: Colors.grey,
                ),
              ),
            ),
            ListTile(
              leading: const Icon(Icons.dashboard),
              title: const Text('GTD Dashboard'),
              onTap: () {
                Navigator.pop(context);
                setState(() => _selectedIndex = 0);
              },
            ),
            ListTile(
              leading: const Icon(Icons.inbox),
              title: const Text('Inbox'),
              onTap: () {
                Navigator.pop(context);
                Navigator.push(
                  context,
                  MaterialPageRoute(builder: (_) => const InboxScreen()),
                );
              },
            ),
            ListTile(
              leading: const Icon(Icons.check_circle_outline),
              title: const Text('Next Actions'),
              onTap: () {
                Navigator.pop(context);
                Navigator.push(
                  context,
                  MaterialPageRoute(builder: (_) => const NextActionsScreen()),
                );
              },
            ),
            ListTile(
              leading: const Icon(Icons.label),
              title: const Text('Contexts'),
              onTap: () {
                Navigator.pop(context);
                Navigator.push(
                  context,
                  MaterialPageRoute(builder: (_) => const ContextsScreen()),
                );
              },
            ),
            ListTile(
              leading: const Icon(Icons.hourglass_empty),
              title: const Text('Waiting For'),
              onTap: () {
                Navigator.pop(context);
                Navigator.push(
                  context,
                  MaterialPageRoute(builder: (_) => const WaitingForScreen()),
                );
              },
            ),
            ListTile(
              leading: const Icon(Icons.lightbulb_outline),
              title: const Text('Someday/Maybe'),
              onTap: () {
                Navigator.pop(context);
                Navigator.push(
                  context,
                  MaterialPageRoute(builder: (_) => const SomedayMaybeScreen()),
                );
              },
            ),
            ListTile(
              leading: const Icon(Icons.assignment),
              title: const Text('Weekly Review'),
              onTap: () {
                Navigator.pop(context);
                Navigator.push(
                  context,
                  MaterialPageRoute(builder: (_) => const WeeklyReviewScreen()),
                );
              },
            ),
            ListTile(
              leading: const Icon(Icons.school),
              title: const Text('My Courses'),
              onTap: () {
                Navigator.pop(context);
                Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (_) => const CourseListScreen(),
                  ),
                );
              },
            ),
            ListTile(
              leading: const Icon(Icons.workspace_premium),
              title: const Text('My Certificates'),
              onTap: () {
                Navigator.pop(context);
                Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (_) => const CertificatesScreen(),
                  ),
                );
              },
            ),
            ListTile(
              leading: const Icon(Icons.schedule),
              title: const Text('Working Schedule'),
              onTap: () {
                Navigator.pop(context);
                Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (_) => const WorkingScheduleScreen(),
                  ),
                );
              },
            ),
            const Divider(),
            ListTile(
              leading: const Icon(Icons.settings),
              title: const Text('Settings'),
              onTap: () {
                Navigator.pop(context);
              },
            ),
            ListTile(
              leading: const Icon(Icons.help_outline),
              title: const Text('Help & Support'),
              onTap: () {
                Navigator.pop(context);
              },
            ),
          ],
        ),
      ),
        );
      },
    );
  }
}