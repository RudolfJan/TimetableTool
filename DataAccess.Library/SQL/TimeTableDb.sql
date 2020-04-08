BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "ServiceInstances" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"ServiceInstanceName"	TEXT NOT NULL,
	"ServiceInstanceAbbreviation"	TEXT NOT NULL,
	"StartTime"	INTEGER NOT NULL,
	"EndTime"	INTEGER NOT NULL,
	"TimeTableId"	INTEGER NOT NULL,
	"ServiceId"	INTEGER NOT NULL,
	FOREIGN KEY("TimeTableId") REFERENCES "TimeTables",
	FOREIGN KEY("ServiceId") REFERENCES "Services"
);
CREATE TABLE IF NOT EXISTS "Timetables" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"TimetableName"	TEXT NOT NULL,
	"TimetableAbbreviation"	TEXT NOT NULL,
	"TimetableDescription"	TEXT NOT NULL,
	"RouteId"	INTEGER NOT NULL,
	FOREIGN KEY("RouteId") REFERENCES "Routes"
);
CREATE TABLE IF NOT EXISTS "TimeEvents" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"EventType"	TEXT NOT NULL,
	"RelativeTime"	INTEGER NOT NULL,
	"LocationId"	INTEGER NOT NULL,
	"ServiceId"	INTEGER NOT NULL,
	"Order" INTEGER NOT NULL,
	FOREIGN KEY("LocationId") REFERENCES "Locations",
	FOREIGN KEY("ServiceId") REFERENCES "Services"
);
CREATE TABLE IF NOT EXISTS "Services" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"ServiceName"	TEXT NOT NULL,
	"ServiceAbbreviation"	TEXT NOT NULL,
	"ServiceType"	TEXT NOT NULL,
	"ServiceDescription"	TEXT NOT NULL,
	"CalculatedDuration"	INTEGER NOT NULL,
	"RouteId"	INTEGER NOT NULL,
	FOREIGN KEY("RouteId") REFERENCES "Routes"
);
CREATE TABLE IF NOT EXISTS "Sections" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"SectionName"	TEXT NOT NULL,
	"NumberOfTracks"	INTEGER NOT NULL,
	"SpeedLimit"	INTEGER NOT NULL,
	"DurationFreight"	INTEGER NOT NULL,
	"DurationPassenger"	INTEGER NOT NULL,
	"SectionLength"	INTEGER NOT NULL,
	"ALocationId"	INTEGER NOT NULL,
	"BLocationId"	INTEGER NOT NULL,
	"BranchId"	INTEGER NOT NULL,
	FOREIGN KEY("ALocationId") REFERENCES "Locations",
	FOREIGN KEY("BLocationId") REFERENCES "Locations",
	FOREIGN KEY("BranchId") REFERENCES "Branches"	
);
CREATE TABLE IF NOT EXISTS "Branches" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"BranchName"	TEXT NOT NULL,
	"BranchAbbreviation"	TEXT NOT NULL,
	"BranchDescription"	TEXT NOT NULL,
	"RouteId"	INTEGER NOT NULL,
	FOREIGN KEY("RouteId") REFERENCES "Routes"
);
CREATE TABLE IF NOT EXISTS "Routes" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"RouteName"	TEXT NOT NULL,
	"RouteAbbreviation"	TEXT NOT NULL,
	"RouteDescription"	TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS "Locations" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"LocationName"	TEXT NOT NULL,
	"LocationAbbreviation" TEXT NOT NULL,
	"NumberOfTracks"	INTEGER NOT NULL,
	"Order"	INTEGER NOT NULL,
	"RouteId"	INTEGER NOT NULL,
	FOREIGN KEY("RouteId") REFERENCES "Routes"
);
COMMIT;
