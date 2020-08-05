BEGIN TRANSACTION;

CREATE TABLE IF NOT EXISTS "ServiceClasses" (
	"Id"	INTEGER NOT NULL,
	"ServiceClassName"	TEXT NOT NULL,
	"ServiceClassDescription"	TEXT NOT NULL,
	"Category" TEXT NOT NULL,
	"Color" TEXT NOT NULL,
	UNIQUE("ServiceClassName"),
	PRIMARY KEY("Id")
	);

CREATE TABLE IF NOT EXISTS "TimeEventTypes" (
	"Id"	INTEGER NOT NULL,
	"EventType"	TEXT NOT NULL,
	"EventTypeDescription"	TEXT NOT NULL DEFAULT '',
	UNIQUE("EventType")
	PRIMARY KEY("Id")
);

CREATE TABLE IF NOT EXISTS "Routes" (
	"Id"	INTEGER NOT NULL PRIMARY KEY,
	"RouteName"	TEXT NOT NULL,
	"RouteAbbreviation"	TEXT NOT NULL,
	"RouteDescription"	TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS "Branches" (
	"Id"	INTEGER NOT NULL PRIMARY KEY,
	"BranchName"	TEXT NOT NULL,
	"BranchAbbreviation"	TEXT NOT NULL,
	"BranchDescription"	TEXT NOT NULL,
	"RouteId"	INTEGER NOT NULL,
	FOREIGN KEY("RouteId") REFERENCES "Routes" ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "Locations" (
	"Id"	INTEGER NOT NULL PRIMARY KEY,
	"LocationName"	TEXT NOT NULL,
	"LocationAbbreviation" TEXT NOT NULL,
	"NumberOfTracks"	INTEGER NOT NULL,
	"Order"	INTEGER NOT NULL,
	"RouteId"	INTEGER NOT NULL,
	FOREIGN KEY("RouteId") REFERENCES "Routes" ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "Sections" (
	"Id"	INTEGER NOT NULL PRIMARY KEY,
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
	"Id" INTEGER NOT NULL PRIMARY KEY,
	"ServiceDirectionName"	TEXT NOT NULL,
	"ServiceDirectionAbbreviation" TEXT NOT NULL,
	"RouteId"	INTEGER NOT NULL,
	"IsDescending" INTEGER NOT NULL DEFAULT 0,
	FOREIGN KEY("RouteId") REFERENCES "Routes" ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ServiceTemplates" (
	"Id"	INTEGER NOT NULL PRIMARY KEY,
	"ServiceTemplateName"	TEXT NOT NULL,
	"ServiceTemplateAbbreviation"	TEXT NOT NULL,
	"ServiceType"	TEXT NOT NULL,
	"ServiceTemplateDescription"	TEXT NOT NULL,
	"ServiceDirectionId" INT NOT NULL,
	"CalculatedDuration"	INTEGER NOT NULL,
	"RouteId"	INTEGER NOT NULL,
	FOREIGN KEY("RouteId") REFERENCES "Routes"  ON DELETE CASCADE,
	FOREIGN KEY("ServiceDirectionId") REFERENCES "ServiceDirections" ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "TimeEvents" (
	"Id"	INTEGER NOT NULL PRIMARY KEY,
	"EventType"	TEXT NOT NULL,
	"ArrivalTime"	INTEGER NOT NULL,
	"WaitTime"	INTEGER NOT NULL DEFAULT 0,
	"LocationId"	INTEGER NOT NULL,
	"ServiceTemplateId"	INTEGER NOT NULL,
	"Order" INTEGER NOT NULL,
	FOREIGN KEY("LocationId") REFERENCES "Locations" ON DELETE CASCADE,
	FOREIGN KEY("ServiceTemplateId") REFERENCES "ServiceTemplates" ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "Timetables" (
	"Id"	INTEGER NOT NULL PRIMARY KEY,
	"TimetableName"	TEXT NOT NULL,
	"TimetableAbbreviation"	TEXT NOT NULL,
	"TimetableDescription"	TEXT NOT NULL,
	"IsMultiDirection" INT NOT NULL DEFAULT 0,
	"ServiceDirectionId"	INT,
	"RouteId"	INTEGER NOT NULL,
	FOREIGN KEY("RouteId") REFERENCES "Routes" ON DELETE CASCADE,
	FOREIGN KEY("ServiceDirectionId") REFERENCES "ServiceDirections" ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "Services" (
	"Id"	INTEGER NOT NULL PRIMARY KEY,
	"ServiceName"	TEXT NOT NULL,
	"ServiceAbbreviation"	TEXT NOT NULL,
	"StartTime"	INTEGER NOT NULL,
	"EndTime"	INTEGER NOT NULL,
	"ServiceTemplateId"	INTEGER NOT NULL,
	FOREIGN KEY("ServiceTemplateId") REFERENCES "ServiceTemplates" ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ConnectTtSi" (
	"Id"	INTEGER NOT NULL PRIMARY KEY,
	"TimetableId"	INTEGER NOT NULL,
	"ServiceId"	INTEGER NOT NULL,
	UNIQUE( "TimetableId","ServiceId"),
	FOREIGN KEY("TimetableId") REFERENCES "Timetables" ON DELETE CASCADE,
	FOREIGN KEY("ServiceId") REFERENCES "Services" ON DELETE CASCADE
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
	,TimeEvents.LocationId AS LocationId
	,TimeEvents.ServiceTemplateId AS ServiceTemplateId
	,ServiceTemplates.ServiceTemplateAbbreviation AS ServiceTemplateAbbreviation
	,ServiceTemplates.ServiceTemplateName AS ServiceTemplateName
FROM TimeEvents, ServiceTemplates, Locations
WHERE ServiceTemplates.Id= TimeEvents.ServiceTemplateId AND Locations.Id= TimeEvents.LocationId
ORDER BY ServiceTemplates.Id ASC, TimeEvents.[Order] ASC;

-- Insert data into configuration tables
DELETE FROM "TimeEventTypes";
INSERT OR IGNORE INTO "TimeEventTypes" ("Id","EventType","EventTypeDescription") VALUES 
 (1,'S','Start service'),
 (2,'E','End service'),
 (3,'P','Pass through location'),
 (4,'H','Halt, pick up passengers'),
 (5,'W','Wait, Intermediate stop'),
 (6,'SE','Shunt activities at single location'),
 (7,'--','No action, just show the location');

DELETE FROM "ServiceClasses";
INSERT INTO "ServiceClasses" ("Id","ServiceClassName","ServiceClassDescription","Category","Color") VALUES 
 (1,'Passenger','Unspecified passenger services','Passenger','Blue'),
 (2,'Freight','Unspecified Freight services','Freight','Red'),
 (3,'Stopping passenger','Stopping passenger service, stops at most stations','Passenger','RoyalBlue'),
 (4,'Regio express passenger','Passenger, limited stops at major stations','Passenger','MediumBlue'),
 (5,'Express passenger','Passenger, stops only at main stations','Passenger','SlateBlue'),
 (6,'International passenger','International passenger trains','Passenger','Indigo'),
 (7,'Passenger depot run','Passenger trains for depot','Passenger','Green'),
 (8,'Light engine','Light engine','Other','Olive'),
 (9,'Freight shunting duty','Freight Shunting','Freight','Orange'),
 (10,'Heavy freight','Heavy freight','Freight','DarkRed'),
 (11,'Express freight','Express freight','Freight','Maroon'),
 (12,'Work train','Work train, track maintenance etcetera','Other','Lime');

COMMIT;
