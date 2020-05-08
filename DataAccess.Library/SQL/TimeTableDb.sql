BEGIN TRANSACTION;

CREATE TABLE IF NOT EXISTS "Routes" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"RouteName"	TEXT NOT NULL,
	"RouteAbbreviation"	TEXT NOT NULL,
	"RouteDescription"	TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS "Branches" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"BranchName"	TEXT NOT NULL,
	"BranchAbbreviation"	TEXT NOT NULL,
	"BranchDescription"	TEXT NOT NULL,
	"RouteId"	INTEGER NOT NULL,
	FOREIGN KEY("RouteId") REFERENCES "Routes" ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "Locations" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"LocationName"	TEXT NOT NULL,
	"LocationAbbreviation" TEXT NOT NULL,
	"NumberOfTracks"	INTEGER NOT NULL,
	"Order"	INTEGER NOT NULL,
	"RouteId"	INTEGER NOT NULL,
	FOREIGN KEY("RouteId") REFERENCES "Routes" ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "Sections" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"SectionName"	TEXT NOT NULL,
	"NumberOfTracks"	INTEGER NOT NULL,
	"SpeedLimit"	INTEGER NOT NULL,
	"Gradient"	INTEGER NOT NULL,
	"DurationFreight"	INTEGER NOT NULL,
	"DurationPassenger"	INTEGER NOT NULL,
	"SectionLength"	INTEGER NOT NULL,
	"ALocationId"	INTEGER NOT NULL,
	"BLocationId"	INTEGER NOT NULL,
	"BranchId"	INTEGER NOT NULL,
	FOREIGN KEY("ALocationId") REFERENCES "Locations" ON DELETE CASCADE,
	FOREIGN KEY("BLocationId") REFERENCES "Locations" ON DELETE CASCADE,
	FOREIGN KEY("BranchId") REFERENCES "Branches" ON DELETE CASCADE	
);

CREATE TABLE IF NOT EXISTS "ServiceDirections" (
	"Id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"ServiceDirectionName"	TEXT NOT NULL,
	"ServiceDirectionAbbreviation" TEXT NOT NULL,
	"RouteId"	INTEGER NOT NULL,
	"IsDescending" INTEGER NOT NULL DEFAULT 0,
	FOREIGN KEY("RouteId") REFERENCES "Routes" ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "Services" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"ServiceName"	TEXT NOT NULL,
	"ServiceAbbreviation"	TEXT NOT NULL,
	"ServiceType"	TEXT NOT NULL,
	"ServiceDescription"	TEXT NOT NULL,
	"ServiceDirectionId" INT NOT NULL,
	"CalculatedDuration"	INTEGER NOT NULL,
	"RouteId"	INTEGER NOT NULL,
	FOREIGN KEY("RouteId") REFERENCES "Routes",
	FOREIGN KEY("ServiceDirectionId") REFERENCES "ServiceDirections" ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "TimeEvents" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"EventType"	TEXT NOT NULL,
	"ArrivalTime"	INTEGER NOT NULL,
	"WaitTime"	INTEGER NOT NULL DEFAULT 0,
	"LocationId"	INTEGER NOT NULL,
	"ServiceId"	INTEGER NOT NULL,
	"Order" INTEGER NOT NULL,
	FOREIGN KEY("LocationId") REFERENCES "Locations" ON DELETE CASCADE,
	FOREIGN KEY("ServiceId") REFERENCES "Services" ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "Timetables" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"TimetableName"	TEXT NOT NULL,
	"TimetableAbbreviation"	TEXT NOT NULL,
	"TimetableDescription"	TEXT NOT NULL,
	"IsMultiDirection" INT NOT NULL DEFAULT 0,
	"ServiceDirectionId"	INT,
	"RouteId"	INTEGER NOT NULL,
	FOREIGN KEY("RouteId") REFERENCES "Routes" ON DELETE CASCADE,
	FOREIGN KEY("ServiceDirectionId") REFERENCES "ServiceDirections" ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ServiceInstances" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"ServiceInstanceName"	TEXT NOT NULL,
	"ServiceInstanceAbbreviation"	TEXT NOT NULL,
	"StartTime"	INTEGER NOT NULL,
	"EndTime"	INTEGER NOT NULL,
	"ServiceId"	INTEGER NOT NULL,
	FOREIGN KEY("ServiceId") REFERENCES "Services" ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ConnectTtSi" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"TimetableId"	INTEGER NOT NULL,
	"ServiceInstanceId"	INTEGER NOT NULL,
	UNIQUE( "TimetableId","ServiceInstanceId"),
	FOREIGN KEY("TimetableId") REFERENCES "Timetables" ON DELETE CASCADE,
	FOREIGN KEY("ServiceInstanceId") REFERENCES "ServiceInstances" ON DELETE CASCADE
);

DROP VIEW IF EXISTS FullTimeEvents;

CREATE VIEW IF NOT EXISTS FullTimeEvents 
AS SELECT
	TimeEvents.Id AS Id
	,Locations.LocationAbbreviation AS LocationAbbreviation
	,Locations.LocationName AS LocationName
	,Locations.NumberOfTracks AS NumberOfTracks
	,Locations.[Order] AS LocationOrder
	,TimeEvents.EventType AS EventType
	,TimeEvents.ArrivalTime AS ArrivalTime
	,TimeEvents.WaitTime AS WaitTime
	,TimeEvents.[Order] AS [Order]
	,Services.ServiceAbbreviation AS ServiceAbbreviation
	,Services.ServiceName AS ServiceName
	,TimeEvents.LocationId AS LocationId
	,TimeEvents.ServiceId AS ServiceId
FROM TimeEvents, Services, Locations
WHERE Services.Id= TimeEvents.ServiceId AND Locations.Id= TimeEvents.LocationId
ORDER BY Services.Id ASC, TimeEvents.[Order] ASC;
COMMIT;
