BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "TimeEventTypes" (
	"Id"	INTEGER NOT NULL,
	"EventType"	TEXT NOT NULL,
	"EventTypeDescription"	TEXT NOT NULL DEFAULT '',
	PRIMARY KEY("Id")
);
INSERT INTO "TimeEventTypes" ("Id","EventType","EventTypeDescription") VALUES (1,'S','Start service'),
 (2,'E','End service'),
 (3,'P','Pass through location'),
 (4,'H','Halt, pick up passengers'),
 (5,'W','Wait, Intermediate stop'),
 (6,'SE','Shunt activities at single location'),
 (7,'--','No action, just show the location');
CREATE VIEW FullTimeEvents 
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
COMMIT;
