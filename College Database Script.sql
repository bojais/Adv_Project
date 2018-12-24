SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

USE [master];
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'College')
    DROP DATABASE College;
GO

-- Create the College database.
CREATE DATABASE College;
GO

-- Specify a simple recovery model 
-- to keep the log growth to a minimum.
ALTER DATABASE College 
    SET RECOVERY SIMPLE;
GO

USE College;
GO





-- Create the Instructor table.
IF NOT EXISTS (SELECT * FROM sys.objects 
        WHERE object_id = OBJECT_ID(N'[dbo].[Instructor]') 
        AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Instructor](
    [InstructorID] [int] IDENTITY(1,1) NOT NULL,
    [LastName] [nvarchar](50) NOT NULL,
    [FirstName] [nvarchar](50) NOT NULL,
    [HireDate] [datetime] NULL,
	[Password] [nvarchar] (10) NOT NULL,
 CONSTRAINT [PK_Instructor] PRIMARY KEY CLUSTERED 
(
    [InstructorID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

-- Create the Student table.
IF NOT EXISTS (SELECT * FROM sys.objects 
        WHERE object_id = OBJECT_ID(N'[dbo].[Student]') 
        AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Student](
    [StudentID] [int] IDENTITY(1,1) NOT NULL,
    [LastName] [nvarchar](50) NOT NULL,
    [FirstName] [nvarchar](50) NOT NULL,
    [EnrollmentDate] [datetime] NULL,
	[Password] [nvarchar] (10) NOT NULL,
 CONSTRAINT [PK_College.Student] PRIMARY KEY CLUSTERED 
(
    [StudentID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

-- Create the TaughtCourse table.
IF NOT EXISTS (SELECT * FROM sys.objects 
        WHERE object_id = OBJECT_ID(N'[dbo].[TaughtCourse]') 
        AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TaughtCourse](
    [TaughtCourseID] [int] IDENTITY(1,1) NOT NULL,
    [CourseID] [int] NOT NULL,
    [Semester] [int] NOT NULL,
    [Year] [nvarchar](50) NOT NULL,
  
 CONSTRAINT [PK_TaughtCourse] PRIMARY KEY CLUSTERED 
(
    [TaughtCourseID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO



--Create the Section table.
IF NOT EXISTS (SELECT * FROM sys.objects 
        WHERE object_id = OBJECT_ID(N'[dbo].[Section]') 
        AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Section](
    [SectionID] [int] IDENTITY(1,1) NOT NULL,
    [TaughtCourseID] [int] NOT NULL,
    [InstructorID] [int] NOT NULL,
	[Capacity] [int] NOT NULL,
 CONSTRAINT [PK_StudentGrade] PRIMARY KEY CLUSTERED 
(
    [SectionID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

-- Create the SectionStudent table.
IF NOT EXISTS (SELECT * FROM sys.objects 
        WHERE object_id = OBJECT_ID(N'[dbo].[SectionStudent]') 
        AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SectionStudent](
    [SectionID] [int] NOT NULL,
    [StudentID] [int] NOT NULL,
    [Grade] [nvarchar](6) NULL,
 CONSTRAINT [PK_College.SectionStudent] PRIMARY KEY CLUSTERED 
(
    [SectionID] ASC,
	[StudentID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

-- Create the Course table.
IF NOT EXISTS (SELECT * FROM sys.objects 
        WHERE object_id = OBJECT_ID(N'[dbo].[Course]') 
        AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Course](
    [CourseID] [int] NOT NULL,
    [Title] [nvarchar](100) NOT NULL,
    [Credits] [int] NOT NULL,
 CONSTRAINT [PK_College.Course] PRIMARY KEY CLUSTERED 
(
    [CourseID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

-- Create the Location table.
IF NOT EXISTS (SELECT * FROM sys.objects 
        WHERE object_id = OBJECT_ID(N'[dbo].[Location]') 
        AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Location](
    [LocationID] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Capacity] [int] NOT NULL,
 CONSTRAINT [PK_College.Location] PRIMARY KEY CLUSTERED 
(
    [LocationID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

-- Create the Schedule table.
IF NOT EXISTS (SELECT * FROM sys.objects 
        WHERE object_id = OBJECT_ID(N'[dbo].[Schedule]') 
        AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Schedule](
    [ScheduleID] [int] IDENTITY(1,1) NOT NULL,
    [SectionID] [int] NOT NULL,
    [LocationID] [int] NOT NULL,
	[Day] [nvarchar](10) NOT NULL,
	[Time] [nvarchar](10) NOT NULL,
	[Duration] [int] NOT NULL,
 CONSTRAINT [PK_College.Schedule] PRIMARY KEY CLUSTERED 
(
    [ScheduleID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

-- Define the relationship between TaughtCourse and Course.
IF NOT EXISTS (SELECT * FROM sys.foreign_keys 
       WHERE object_id = OBJECT_ID(N'[dbo].[FK_TaughtCourse_Course]')
       AND parent_object_id = OBJECT_ID(N'[dbo].[TaughtCourse]'))
ALTER TABLE [dbo].[TaughtCourse]  WITH CHECK ADD  
       CONSTRAINT [FK_TaughtCourse_Course] FOREIGN KEY([CourseID])
REFERENCES [dbo].[Course] ([CourseID])
GO
ALTER TABLE [dbo].[TaughtCourse] CHECK 
       CONSTRAINT [FK_TaughtCourse_Course]
GO

-- Define the relationship between Section and Instructor
IF NOT EXISTS (SELECT * FROM sys.foreign_keys 
       WHERE object_id = OBJECT_ID(N'[dbo].[Section]')
       AND parent_object_id = OBJECT_ID(N'[dbo].[Section]'))
ALTER TABLE [dbo].[Section]  WITH CHECK ADD  
       CONSTRAINT [FK_Section_Instructor] FOREIGN KEY([InstructorID])
REFERENCES [dbo].[Instructor] ([InstructorID])
GO
ALTER TABLE [dbo].[Section] CHECK 
       CONSTRAINT [FK_Section_Instructor]
GO









--Define the relationship between Section and TaughtCourse.
IF NOT EXISTS (SELECT * FROM sys.foreign_keys 
       WHERE object_id = OBJECT_ID(N'[dbo].[FK_StudentGrade_Student]')
       AND parent_object_id = OBJECT_ID(N'[dbo].[Section]'))
ALTER TABLE [dbo].[Section]  WITH CHECK ADD  
       CONSTRAINT [FK_Section_Student] FOREIGN KEY([TaughtCourseID])
REFERENCES [dbo].[TaughtCourse] ([TaughtCourseID])
GO
ALTER TABLE [dbo].[Section] CHECK 
       CONSTRAINT [FK_Section_Student]
GO



--Define the relationship between SectionStudent and Section.
IF NOT EXISTS (SELECT * FROM sys.foreign_keys 
       WHERE object_id = OBJECT_ID(N'[dbo].[FK_SectionStudent_Section]')
       AND parent_object_id = OBJECT_ID(N'[dbo].[SectionStudent]'))
ALTER TABLE [dbo].[SectionStudent]  WITH CHECK ADD  
       CONSTRAINT [FK_SectionStudent_Section] FOREIGN KEY([SectionID])
REFERENCES [dbo].[Section] ([SectionID])
GO
ALTER TABLE [dbo].[SectionStudent] CHECK 
       CONSTRAINT [FK_SectionStudent_Section]
GO

--Define the relationship between SectionStudent and Student.
IF NOT EXISTS (SELECT * FROM sys.foreign_keys 
       WHERE object_id = OBJECT_ID(N'[dbo].[FK_SectionStudent_Student]')
       AND parent_object_id = OBJECT_ID(N'[dbo].[SectionStudent]'))
ALTER TABLE [dbo].[SectionStudent]  WITH CHECK ADD  
       CONSTRAINT [FK_SectionStudent_Student] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
GO
ALTER TABLE [dbo].[SectionStudent] CHECK 
       CONSTRAINT [FK_SectionStudent_Student]
GO

--Define the relationship between Schedule and Section.
IF NOT EXISTS (SELECT * FROM sys.foreign_keys 
       WHERE object_id = OBJECT_ID(N'[dbo].[FK_Schedule_Section]')
       AND parent_object_id = OBJECT_ID(N'[dbo].[Schedule]'))
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD  
       CONSTRAINT [FK_Schedule_Section] FOREIGN KEY([SectionID])
REFERENCES [dbo].[Section] ([SectionID])
GO
ALTER TABLE [dbo].[Schedule] CHECK 
       CONSTRAINT [FK_Schedule_Section]
GO


--Define the relationship between Schedule and Location.
IF NOT EXISTS (SELECT * FROM sys.foreign_keys 
       WHERE object_id = OBJECT_ID(N'[dbo].[FK_Schedule_Location]')
       AND parent_object_id = OBJECT_ID(N'[dbo].[Schedule]'))
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD  
       CONSTRAINT [FK_Schedule_Location] FOREIGN KEY([LocationID])
REFERENCES [dbo].[Location] ([LocationID])
GO
ALTER TABLE [dbo].[Schedule] CHECK 
       CONSTRAINT [FK_Schedule_Location]
GO

-- Insert data into the Student table.
USE College
GO
SET IDENTITY_INSERT dbo.Student ON
GO

INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (2, 'Barzdukas', 'Gytis', '2005-09-01', 'kjmslp');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (3, 'Justice', 'Peggy', '2001-09-01', 'hxmnwh');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (6, 'Li', 'Yan', '2002-09-01', 'nuiopa');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (7, 'Norman', 'Laura', '2003-09-01', 'mlpdbh');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (8, 'Olivotto', 'Nino', '2005-09-01', 'nxvpaq');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (9, 'Tang', 'Wayne', '2005-09-01', 'mgotzd');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (10, 'Alonso', 'Meredith', '2002-09-01', 'cxzuix');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (11, 'Lopez', 'Sophia', '2004-09-01', 'mptrfv');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (12, 'Browning', 'Meredith', '2000-09-01', 'lgkeit');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (13, 'Anand', 'Arturo', '2003-09-01', 'nxzpha');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (14, 'Walker', 'Alexandra', '2000-09-01', 'ntyuxc');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (15, 'Powell', 'Carson', '2004-09-01', 'miutvz');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (16, 'Jai', 'Damien', '2001-09-01', 'nxitgf');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (17, 'Carlson', 'Robyn', '2005-09-01', 'mmhtwe');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (19, 'Bryant', 'Carson', '2001-09-01', 'lkddju');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (20, 'Suarez', 'Robyn', '2004-09-01', 'nntyxs');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (21, 'Holt', 'Roger', '2004-09-01', 'niutcd');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (22, 'Alexander', 'Carson', '2005-09-01', 'mmhtza');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (23, 'Morgan', 'Isaiah', '2001-09-01', 'pputxw');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (24, 'Martin', 'Randall', '2005-09-01', 'vxcpot');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (26, 'Rogers', 'Cody', '2002-09-01', 'llprcq');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (28, 'White', 'Anthony', '2001-09-01', 'qqmitz');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (29, 'Griffin', 'Rachel', '2004-09-01', 'kkhtxw');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (30, 'Shan', 'Alicia', '2003-09-01', 'bbvtee');
INSERT INTO dbo.Student (StudentID, LastName, FirstName, EnrollmentDate, Password)
VALUES (33, 'Gao', 'Erica', '2003-01-30', 'llktfd');
GO
SET IDENTITY_INSERT dbo.Student OFF
GO





-- Insert data into the Course table.
INSERT INTO dbo.Course (CourseID, Title, Credits)
VALUES (1050, 'Chemistry', 15);
INSERT INTO dbo.Course (CourseID, Title, Credits)
VALUES (1061, 'Physics', 15);
INSERT INTO dbo.Course (CourseID, Title, Credits)
VALUES (1045, 'Calculus', 15);
INSERT INTO dbo.Course (CourseID, Title, Credits)
VALUES (2030, 'Poetry', 15);
INSERT INTO dbo.Course (CourseID, Title, Credits)
VALUES (2021, 'Composition', 10);
INSERT INTO dbo.Course (CourseID, Title, Credits)
VALUES (2042, 'Literature', 15);
INSERT INTO dbo.Course (CourseID, Title, Credits)
VALUES (4022, 'Microeconomics', 15);
INSERT INTO dbo.Course (CourseID, Title, Credits)
VALUES (4041, 'Macroeconomics', 15);
INSERT INTO dbo.Course (CourseID, Title, Credits)
VALUES (4061, 'Quantitative Analysis', 15);
INSERT INTO dbo.Course (CourseID, Title, Credits)
VALUES (3141, 'Trigonometry', 15);
GO









-- Insert data into the Instructor table.
USE College
GO
SET IDENTITY_INSERT dbo.Instructor ON
GO
INSERT INTO dbo.Instructor (InstructorID, LastName, FirstName, HireDate, Password)
VALUES (1, 'Abercrombie', 'Kim', '1995-03-11', 'gtwge');
INSERT INTO dbo.Instructor (InstructorID, LastName, FirstName, HireDate, Password)
VALUES (4, 'Fakhouri', 'Fadi', '2002-08-06', 'bhwia');
INSERT INTO dbo.Instructor (InstructorID, LastName, FirstName, HireDate, Password)
VALUES (5, 'Harui', 'Roger', '1998-07-01', 'vqkar');
INSERT INTO dbo.Instructor (InstructorID, LastName, FirstName, HireDate, Password)
VALUES (18, 'Zheng', 'Roger', '2004-02-12', 'gpwan');
INSERT INTO dbo.Instructor (InstructorID, LastName, FirstName, HireDate, Password)
VALUES (25, 'Kapoor', 'Candace', '2001-01-15', 'mgpag');
INSERT INTO dbo.Instructor (InstructorID, LastName, FirstName, HireDate, Password)
VALUES (27, 'Serrano', 'Stacy', '1999-06-01', 'cxamn');
INSERT INTO dbo.Instructor (InstructorID, LastName, FirstName, HireDate, Password)
VALUES (31, 'Stewart', 'Jasmine', '1997-10-12', 'vpowe');
INSERT INTO dbo.Instructor (InstructorID, LastName, FirstName, HireDate, Password)
VALUES (32, 'Xu', 'Kristen', '2001-7-23', 'lbmht');
INSERT INTO dbo.Instructor (InstructorID, LastName, FirstName, HireDate, Password)
VALUES (34, 'Van Houten', 'Roger', '2000-12-07', 'nbfcx');
GO
SET IDENTITY_INSERT dbo.Instructor OFF
GO

--Insert data into TaughtCourse table.
USE College
GO
SET IDENTITY_INSERT dbo.TaughtCourse ON
GO
INSERT INTO dbo.TaughtCourse (TaughtCourseID, CourseID, Semester, Year)
VALUES (1, 1050, 1, '2015-16');
INSERT INTO dbo.TaughtCourse (TaughtCourseID, CourseID, Semester, Year)
VALUES (2, 1061, 1, '2015-16');
INSERT INTO dbo.TaughtCourse (TaughtCourseID, CourseID, Semester, Year)
VALUES (3,1045, 1, '2015-16');
INSERT INTO dbo.TaughtCourse (TaughtCourseID, CourseID, Semester, Year)
VALUES (4, 4061, 1, '2015-16');
INSERT INTO dbo.TaughtCourse (TaughtCourseID, CourseID, Semester, Year)
VALUES (5, 2042, 1, '2015-16');
INSERT INTO dbo.TaughtCourse (TaughtCourseID, CourseID, Semester, Year)
VALUES (6, 4022, 1, '2015-16');
GO
SET IDENTITY_INSERT dbo.TaughtCourse OFF
GO

USE College
GO
SET IDENTITY_INSERT dbo.Section ON
GO
-- Insert data into the Section table.
INSERT INTO dbo.Section (SectionID, TaughtCourseID, InstructorID, Capacity)
VALUES (1, 1, 1, 20);
INSERT INTO dbo.Section (SectionID, TaughtCourseID, InstructorID, Capacity)
VALUES (2, 1, 4, 20);
INSERT INTO dbo.Section (SectionID, TaughtCourseID, InstructorID, Capacity)
VALUES (3, 2, 5, 15);
INSERT INTO dbo.Section (SectionID, TaughtCourseID, InstructorID, Capacity)
VALUES (4, 3, 18, 20);
INSERT INTO dbo.Section (SectionID, TaughtCourseID, InstructorID, Capacity)
VALUES (5, 4, 25, 18);
INSERT INTO dbo.Section (SectionID, TaughtCourseID, InstructorID, Capacity)
VALUES (6, 5, 27, 20);
INSERT INTO dbo.Section (SectionID, TaughtCourseID, InstructorID, Capacity)
VALUES (7, 6, 31, 20);
INSERT INTO dbo.Section (SectionID, TaughtCourseID, InstructorID, Capacity)
VALUES (8, 6, 32, 10);
INSERT INTO dbo.Section (SectionID, TaughtCourseID, InstructorID, Capacity)
VALUES (9, 6, 34, 10);
GO
SET IDENTITY_INSERT dbo.Section OFF
GO

-- Insert data into the SectionStudent table.
INSERT INTO dbo.SectionStudent (SectionID, StudentID, Grade)
VALUES (1, 2, 45);
INSERT INTO dbo.SectionStudent (SectionID, StudentID, Grade)
VALUES (1, 3, 97.5);
INSERT INTO dbo.SectionStudent (SectionID, StudentID, Grade)
VALUES (1, 6, 48);
INSERT INTO dbo.SectionStudent (SectionID, StudentID, Grade)
VALUES (1, 7, 36);
INSERT INTO dbo.SectionStudent (SectionID, StudentID, Grade)
VALUES (1, 8, 65);
INSERT INTO dbo.SectionStudent (SectionID, StudentID, Grade)
VALUES (1, 9, 77.5);
INSERT INTO dbo.SectionStudent (SectionID, StudentID, Grade)
VALUES (2, 28, 98);
INSERT INTO dbo.SectionStudent (SectionID, StudentID, Grade)
VALUES (2, 29, 56);
INSERT INTO dbo.SectionStudent (SectionID, StudentID, Grade)
VALUES (2, 30, 68);
INSERT INTO dbo.SectionStudent (SectionID, StudentID, Grade)
VALUES (2, 33, 72);
GO

USE College
GO
SET IDENTITY_INSERT dbo.Location ON
GO
-- Insert data into the Location table.
INSERT INTO dbo.Location (LocationID, Name, Capacity)
VALUES (1, 20.001, 20);
INSERT INTO dbo.Location (LocationID, Name, Capacity)
VALUES (2, 20.002, 20);
INSERT INTO dbo.Location (LocationID, Name, Capacity)
VALUES (3, 10.001, 15);
INSERT INTO dbo.Location (LocationID, Name, Capacity)
VALUES (4, 10.002, 20);
INSERT INTO dbo.Location (LocationID, Name, Capacity)
VALUES (5, 10.003, 20);
INSERT INTO dbo.Location (LocationID, Name, Capacity)
VALUES (6, 12.001, 20);
INSERT INTO dbo.Location (LocationID, Name, Capacity)
VALUES (7, 12.002, 20);
INSERT INTO dbo.Location (LocationID, Name, Capacity)
VALUES (8, 12.003, 15);
INSERT INTO dbo.Location (LocationID, Name, Capacity)
VALUES (9, 12.004, 15);
GO
SET IDENTITY_INSERT dbo.Location OFF
GO

USE College
GO
SET IDENTITY_INSERT dbo.Schedule ON
GO
-- Insert data into the Schedule table.
INSERT INTO dbo.Schedule (ScheduleID, SectionID, LocationID, Day, Time, Duration)
VALUES (1, 1, 1, 'Monday', '10', 2);
INSERT INTO dbo.Schedule (ScheduleID, SectionID, LocationID, Day, Time, Duration)
VALUES (2, 1, 1, 'Wednesday', '2', 2);
INSERT INTO dbo.Schedule (ScheduleID, SectionID, LocationID, Day, Time, Duration)
VALUES (3, 1, 1, 'Thursday', '10', 2);
INSERT INTO dbo.Schedule (ScheduleID, SectionID, LocationID, Day, Time, Duration)
VALUES (4, 2, 2, 'Monday', '10', 2);
INSERT INTO dbo.Schedule (ScheduleID, SectionID, LocationID, Day, Time, Duration)
VALUES (5, 2, 2, 'Wednesday', '2', 2);
INSERT INTO dbo.Schedule (ScheduleID, SectionID, LocationID, Day, Time, Duration)
VALUES (6, 2, 2, 'Thursday', '10', 2);
GO
SET IDENTITY_INSERT dbo.Schedule OFF
GO