USE [master]
GO
/****** Object:  Database [prbd_1718_presences_g27]    Script Date: 11-03-18 17:07:07 ******/
CREATE DATABASE [prbd_1718_presences_g27]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'prbd_1718_presences_g27', FILENAME = N'{DBPATH}\prbd_1718_presences_g27.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'prbd_1718_presences_g27_log', FILENAME = N'{DBPATH}\prbd_1718_presences_g27_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [prbd_1718_presences_g27].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [prbd_1718_presences_g27] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET ARITHABORT OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET  ENABLE_BROKER 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET RECOVERY FULL 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET  MULTI_USER 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [prbd_1718_presences_g27] SET DB_CHAINING OFF 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [prbd_1718_presences_g27] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'prbd_1718_presences_g27', N'ON'
GO
USE [prbd_1718_presences_g27]
GO
/****** Object:  Schema [m2ss]    Script Date: 11-03-18 17:07:07 ******/
CREATE SCHEMA [m2ss]
GO
/****** Object:  Table [dbo].[certificate]    Script Date: 11-03-18 17:07:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[certificate](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[student] [int] NOT NULL,
	[startdate] [date] NOT NULL,
	[finishdate] [date] NOT NULL,
 CONSTRAINT [PK_certificate_id] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[course]    Script Date: 11-03-18 17:07:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[course](
	[code] [int] NOT NULL,
	[title] [nvarchar](128) NOT NULL,
	[starttime] [time](7) NOT NULL,
	[endtime] [time](7) NOT NULL,
	[startdate] [date] NOT NULL,
	[finishdate] [date] NOT NULL,
	[teacher] [int] NOT NULL,
	[dayofweek] [int] NOT NULL,
 CONSTRAINT [PK_course_code] PRIMARY KEY CLUSTERED 
(
	[code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[courseoccurrence]    Script Date: 11-03-18 17:07:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[courseoccurrence](
	[id] [int] IDENTITY(125,1) NOT NULL,
	[date] [date] NOT NULL,
	[course] [int] NOT NULL,
 CONSTRAINT [PK_courseoccurrence_id] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[presence]    Script Date: 11-03-18 17:07:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[presence](
	[student] [int] NOT NULL,
	[courseoccurence] [int] NOT NULL,
	[present] [smallint] NOT NULL,
 CONSTRAINT [PK_presence_student] PRIMARY KEY CLUSTERED 
(
	[student] ASC,
	[courseoccurence] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[student]    Script Date: 11-03-18 17:07:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[student](
	[id] [int] IDENTITY(11,1) NOT NULL,
	[lastname] [nvarchar](128) NOT NULL,
	[firstname] [nvarchar](128) NOT NULL,
	[sex] [nvarchar](1) NOT NULL,
 CONSTRAINT [PK_student_id] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[studentcourses]    Script Date: 11-03-18 17:07:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[studentcourses](
	[student] [int] NOT NULL,
	[course] [int] NOT NULL,
 CONSTRAINT [PK_studentcourses_student] PRIMARY KEY CLUSTERED 
(
	[student] ASC,
	[course] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user]    Script Date: 11-03-18 17:07:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user](
	[id] [int] IDENTITY(5,1) NOT NULL,
	[pseudo] [nvarchar](128) NOT NULL,
	[password] [nvarchar](128) NOT NULL,
	[fullname] [nvarchar](128) NOT NULL,
	[role] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_user_id] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[course] ([code], [title], [starttime], [endtime], [startdate], [finishdate], [teacher], [dayofweek]) VALUES (1111, N'PRWB - Projet de développement Web', CAST(N'15:00:00' AS Time), CAST(N'17:30:00' AS Time), CAST(N'2017-09-18' AS Date), CAST(N'2018-06-30' AS Date), 1, 0)
GO
INSERT [dbo].[course] ([code], [title], [starttime], [endtime], [startdate], [finishdate], [teacher], [dayofweek]) VALUES (2222, N'PRWB - Projet de développement Web', CAST(N'10:00:00' AS Time), CAST(N'12:30:00' AS Time), CAST(N'2017-09-18' AS Date), CAST(N'2018-06-30' AS Date), 2, 1)
GO
INSERT [dbo].[course] ([code], [title], [starttime], [endtime], [startdate], [finishdate], [teacher], [dayofweek]) VALUES (3333, N'WEB1 - Principes de base Web', CAST(N'13:00:00' AS Time), CAST(N'15:00:00' AS Time), CAST(N'2018-02-05' AS Date), CAST(N'2018-06-30' AS Date), 2, 1)
GO
INSERT [dbo].[course] ([code], [title], [starttime], [endtime], [startdate], [finishdate], [teacher], [dayofweek]) VALUES (4444, N'PRM2 - Principes algorithmiques et programmation', CAST(N'13:00:00' AS Time), CAST(N'17:00:00' AS Time), CAST(N'2018-02-05' AS Date), CAST(N'2018-06-30' AS Date), 1, 3)
GO
SET IDENTITY_INSERT [dbo].[courseoccurrence] ON 
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (1, CAST(N'2017-09-18' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (42, CAST(N'2017-09-19' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (2, CAST(N'2017-09-25' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (43, CAST(N'2017-09-26' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (3, CAST(N'2017-10-02' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (44, CAST(N'2017-10-03' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (4, CAST(N'2017-10-09' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (45, CAST(N'2017-10-10' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (5, CAST(N'2017-10-16' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (46, CAST(N'2017-10-17' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (6, CAST(N'2017-10-23' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (47, CAST(N'2017-10-24' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (7, CAST(N'2017-10-30' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (48, CAST(N'2017-10-31' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (8, CAST(N'2017-11-06' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (49, CAST(N'2017-11-07' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (9, CAST(N'2017-11-13' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (50, CAST(N'2017-11-14' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (10, CAST(N'2017-11-20' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (51, CAST(N'2017-11-21' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (11, CAST(N'2017-11-27' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (52, CAST(N'2017-11-28' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (12, CAST(N'2017-12-04' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (53, CAST(N'2017-12-05' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (13, CAST(N'2017-12-11' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (54, CAST(N'2017-12-12' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (14, CAST(N'2017-12-18' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (55, CAST(N'2017-12-19' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (15, CAST(N'2017-12-25' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (56, CAST(N'2017-12-26' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (16, CAST(N'2018-01-01' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (57, CAST(N'2018-01-02' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (17, CAST(N'2018-01-08' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (58, CAST(N'2018-01-09' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (18, CAST(N'2018-01-15' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (59, CAST(N'2018-01-16' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (19, CAST(N'2018-01-22' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (60, CAST(N'2018-01-23' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (20, CAST(N'2018-01-29' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (61, CAST(N'2018-01-30' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (21, CAST(N'2018-02-05' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (62, CAST(N'2018-02-06' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (83, CAST(N'2018-02-06' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (104, CAST(N'2018-02-08' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (22, CAST(N'2018-02-12' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (63, CAST(N'2018-02-13' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (84, CAST(N'2018-02-13' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (105, CAST(N'2018-02-15' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (23, CAST(N'2018-02-19' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (64, CAST(N'2018-02-20' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (85, CAST(N'2018-02-20' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (106, CAST(N'2018-02-22' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (24, CAST(N'2018-02-26' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (65, CAST(N'2018-02-27' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (86, CAST(N'2018-02-27' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (107, CAST(N'2018-03-01' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (25, CAST(N'2018-03-05' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (66, CAST(N'2018-03-06' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (87, CAST(N'2018-03-06' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (108, CAST(N'2018-03-08' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (26, CAST(N'2018-03-12' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (67, CAST(N'2018-03-13' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (88, CAST(N'2018-03-13' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (109, CAST(N'2018-03-15' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (27, CAST(N'2018-03-19' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (68, CAST(N'2018-03-20' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (89, CAST(N'2018-03-20' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (110, CAST(N'2018-03-22' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (28, CAST(N'2018-03-26' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (69, CAST(N'2018-03-27' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (90, CAST(N'2018-03-27' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (111, CAST(N'2018-03-29' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (29, CAST(N'2018-04-02' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (70, CAST(N'2018-04-03' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (91, CAST(N'2018-04-03' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (112, CAST(N'2018-04-05' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (30, CAST(N'2018-04-09' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (71, CAST(N'2018-04-10' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (92, CAST(N'2018-04-10' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (113, CAST(N'2018-04-12' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (31, CAST(N'2018-04-16' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (72, CAST(N'2018-04-17' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (93, CAST(N'2018-04-17' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (114, CAST(N'2018-04-19' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (32, CAST(N'2018-04-23' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (73, CAST(N'2018-04-24' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (94, CAST(N'2018-04-24' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (115, CAST(N'2018-04-26' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (33, CAST(N'2018-04-30' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (74, CAST(N'2018-05-01' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (95, CAST(N'2018-05-01' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (116, CAST(N'2018-05-03' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (34, CAST(N'2018-05-07' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (75, CAST(N'2018-05-08' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (96, CAST(N'2018-05-08' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (117, CAST(N'2018-05-10' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (35, CAST(N'2018-05-14' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (76, CAST(N'2018-05-15' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (97, CAST(N'2018-05-15' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (118, CAST(N'2018-05-17' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (36, CAST(N'2018-05-21' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (77, CAST(N'2018-05-22' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (98, CAST(N'2018-05-22' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (119, CAST(N'2018-05-24' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (37, CAST(N'2018-05-28' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (78, CAST(N'2018-05-29' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (99, CAST(N'2018-05-29' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (120, CAST(N'2018-05-31' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (38, CAST(N'2018-06-04' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (79, CAST(N'2018-06-05' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (100, CAST(N'2018-06-05' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (121, CAST(N'2018-06-07' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (39, CAST(N'2018-06-11' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (80, CAST(N'2018-06-12' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (101, CAST(N'2018-06-12' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (122, CAST(N'2018-06-14' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (40, CAST(N'2018-06-18' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (81, CAST(N'2018-06-19' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (102, CAST(N'2018-06-19' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (123, CAST(N'2018-06-21' AS Date), 4444)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (41, CAST(N'2018-06-25' AS Date), 1111)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (82, CAST(N'2018-06-26' AS Date), 2222)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (103, CAST(N'2018-06-26' AS Date), 3333)
GO
INSERT [dbo].[courseoccurrence] ([id], [date], [course]) VALUES (124, CAST(N'2018-06-28' AS Date), 4444)
GO
SET IDENTITY_INSERT [dbo].[courseoccurrence] OFF
GO
SET IDENTITY_INSERT [dbo].[student] ON 
GO
INSERT [dbo].[student] ([id], [lastname], [firstname], [sex]) VALUES (1, N'Aragon', N'Louis', N'M')
GO
INSERT [dbo].[student] ([id], [lastname], [firstname], [sex]) VALUES (2, N'Dylan', N'Bob', N'M')
GO
INSERT [dbo].[student] ([id], [lastname], [firstname], [sex]) VALUES (3, N'Gréco', N'Juliette', N'F')
GO
INSERT [dbo].[student] ([id], [lastname], [firstname], [sex]) VALUES (4, N'Piaf', N'Edith', N'F')
GO
INSERT [dbo].[student] ([id], [lastname], [firstname], [sex]) VALUES (5, N'Macias', N'Enrico', N'M')
GO
INSERT [dbo].[student] ([id], [lastname], [firstname], [sex]) VALUES (6, N'Delerm', N'Vincent', N'M')
GO
INSERT [dbo].[student] ([id], [lastname], [firstname], [sex]) VALUES (7, N'Cherhal', N'Jeanne', N'F')
GO
INSERT [dbo].[student] ([id], [lastname], [firstname], [sex]) VALUES (8, N'Dion', N'Céline', N'F')
GO
INSERT [dbo].[student] ([id], [lastname], [firstname], [sex]) VALUES (9, N'Franklin', N'Aretha', N'F')
GO
INSERT [dbo].[student] ([id], [lastname], [firstname], [sex]) VALUES (10, N'Sardou', N'Michel', N'M')
GO
SET IDENTITY_INSERT [dbo].[student] OFF
GO
INSERT [dbo].[studentcourses] ([student], [course]) VALUES (3, 1111)
GO
INSERT [dbo].[studentcourses] ([student], [course]) VALUES (6, 1111)
GO
INSERT [dbo].[studentcourses] ([student], [course]) VALUES (7, 1111)
GO
INSERT [dbo].[studentcourses] ([student], [course]) VALUES (8, 1111)
GO
INSERT [dbo].[studentcourses] ([student], [course]) VALUES (4, 2222)
GO
INSERT [dbo].[studentcourses] ([student], [course]) VALUES (5, 2222)
GO
INSERT [dbo].[studentcourses] ([student], [course]) VALUES (9, 2222)
GO
INSERT [dbo].[studentcourses] ([student], [course]) VALUES (10, 2222)
GO
INSERT [dbo].[studentcourses] ([student], [course]) VALUES (1, 3333)
GO
INSERT [dbo].[studentcourses] ([student], [course]) VALUES (2, 3333)
GO
INSERT [dbo].[studentcourses] ([student], [course]) VALUES (3, 3333)
GO
INSERT [dbo].[studentcourses] ([student], [course]) VALUES (1, 4444)
GO
INSERT [dbo].[studentcourses] ([student], [course]) VALUES (2, 4444)
GO
INSERT [dbo].[studentcourses] ([student], [course]) VALUES (3, 4444)
GO
INSERT [dbo].[studentcourses] ([student], [course]) VALUES (4, 4444)
GO
INSERT [dbo].[studentcourses] ([student], [course]) VALUES (5, 4444)
GO
SET IDENTITY_INSERT [dbo].[user] ON 
GO
INSERT [dbo].[user] ([id], [pseudo], [password], [fullname], [role]) VALUES (1, N'boris', N'password', N'Boris Verhaegen', N'teacher')
GO
INSERT [dbo].[user] ([id], [pseudo], [password], [fullname], [role]) VALUES (2, N'benoit', N'password', N'Benoit Penelle', N'teacher')
GO
INSERT [dbo].[user] ([id], [pseudo], [password], [fullname], [role]) VALUES (3, N'stephanie', N'password', N'Stéphanie', N'admin')
GO
INSERT [dbo].[user] ([id], [pseudo], [password], [fullname], [role]) VALUES (4, N'alain', N'password', N'Alain Silovy', N'teacher')
GO
INSERT [dbo].[user] ([id], [pseudo], [password], [fullname], [role]) VALUES (5, N'bruno', N'password', N'Bruno Lacroix', N'teacher')
GO
SET IDENTITY_INSERT [dbo].[user] OFF
GO
/****** Object:  Index [student]    Script Date: 11-03-18 17:07:07 ******/
CREATE NONCLUSTERED INDEX [student] ON [dbo].[certificate]
(
	[student] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [teacher]    Script Date: 11-03-18 17:07:07 ******/
CREATE NONCLUSTERED INDEX [teacher] ON [dbo].[course]
(
	[teacher] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [courseoccurrence$date]    Script Date: 11-03-18 17:07:07 ******/
ALTER TABLE [dbo].[courseoccurrence] ADD  CONSTRAINT [courseoccurrence$date] UNIQUE NONCLUSTERED 
(
	[date] ASC,
	[course] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [courseoccurrence$date_2]    Script Date: 11-03-18 17:07:07 ******/
ALTER TABLE [dbo].[courseoccurrence] ADD  CONSTRAINT [courseoccurrence$date_2] UNIQUE NONCLUSTERED 
(
	[date] ASC,
	[course] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [course]    Script Date: 11-03-18 17:07:07 ******/
CREATE NONCLUSTERED INDEX [course] ON [dbo].[courseoccurrence]
(
	[course] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [presence$student]    Script Date: 11-03-18 17:07:07 ******/
ALTER TABLE [dbo].[presence] ADD  CONSTRAINT [presence$student] UNIQUE NONCLUSTERED 
(
	[student] ASC,
	[courseoccurence] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [courseoccurence]    Script Date: 11-03-18 17:07:07 ******/
CREATE NONCLUSTERED INDEX [courseoccurence] ON [dbo].[presence]
(
	[courseoccurence] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [student_2]    Script Date: 11-03-18 17:07:07 ******/
CREATE NONCLUSTERED INDEX [student_2] ON [dbo].[presence]
(
	[student] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [course]    Script Date: 11-03-18 17:07:07 ******/
CREATE NONCLUSTERED INDEX [course] ON [dbo].[studentcourses]
(
	[course] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [user$pseudo]    Script Date: 11-03-18 17:07:07 ******/
ALTER TABLE [dbo].[user] ADD  CONSTRAINT [user$pseudo] UNIQUE NONCLUSTERED 
(
	[pseudo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[certificate]  WITH NOCHECK ADD  CONSTRAINT [certificate$certificate_ibfk_1] FOREIGN KEY([student])
REFERENCES [dbo].[student] ([id])
GO
ALTER TABLE [dbo].[certificate] CHECK CONSTRAINT [certificate$certificate_ibfk_1]
GO
ALTER TABLE [dbo].[course]  WITH NOCHECK ADD  CONSTRAINT [course$course_ibfk_1] FOREIGN KEY([teacher])
REFERENCES [dbo].[user] ([id])
GO
ALTER TABLE [dbo].[course] CHECK CONSTRAINT [course$course_ibfk_1]
GO
ALTER TABLE [dbo].[courseoccurrence]  WITH NOCHECK ADD  CONSTRAINT [courseoccurrence$courseoccurrence_ibfk_1] FOREIGN KEY([course])
REFERENCES [dbo].[course] ([code])
GO
ALTER TABLE [dbo].[courseoccurrence] CHECK CONSTRAINT [courseoccurrence$courseoccurrence_ibfk_1]
GO
ALTER TABLE [dbo].[presence]  WITH NOCHECK ADD  CONSTRAINT [presence$presence_ibfk_1] FOREIGN KEY([student])
REFERENCES [dbo].[student] ([id])
GO
ALTER TABLE [dbo].[presence] CHECK CONSTRAINT [presence$presence_ibfk_1]
GO
ALTER TABLE [dbo].[presence]  WITH NOCHECK ADD  CONSTRAINT [presence$presence_ibfk_2] FOREIGN KEY([courseoccurence])
REFERENCES [dbo].[courseoccurrence] ([id])
GO
ALTER TABLE [dbo].[presence] CHECK CONSTRAINT [presence$presence_ibfk_2]
GO
ALTER TABLE [dbo].[studentcourses]  WITH NOCHECK ADD  CONSTRAINT [studentcourses$studentcourses_ibfk_1] FOREIGN KEY([student])
REFERENCES [dbo].[student] ([id])
GO
ALTER TABLE [dbo].[studentcourses] CHECK CONSTRAINT [studentcourses$studentcourses_ibfk_1]
GO
ALTER TABLE [dbo].[studentcourses]  WITH NOCHECK ADD  CONSTRAINT [studentcourses$studentcourses_ibfk_2] FOREIGN KEY([course])
REFERENCES [dbo].[course] ([code])
GO
ALTER TABLE [dbo].[studentcourses] CHECK CONSTRAINT [studentcourses$studentcourses_ibfk_2]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'prwb_1718_gxx.certificate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'certificate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'prwb_1718_gxx.course' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'course'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'prwb_1718_gxx.courseoccurrence' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'courseoccurrence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'prwb_1718_gxx.presence' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'presence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'prwb_1718_gxx.student' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'student'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'prwb_1718_gxx.studentcourses' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'studentcourses'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'prwb_1718_gxx.`user`' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user'
GO
USE [master]
GO
ALTER DATABASE [prbd_1718_presences_g27] SET  READ_WRITE 
GO
