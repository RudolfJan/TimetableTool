BEGIN TRANSACTION;
DROP VIEW IF EXISTS FullTimeEvents;

ALTER TABLE Services RENAME TO ServiceTemplates;
ALTER TABLE TimeEvents RENAME ServiceId TO ServiceTemplateId;
ALTER TABLE ServiceInstances RENAME ServiceId TO ServiceTemplateId;
ALTER TABLE ServiceTemplates RENAME ServiceAbbreviation TO ServiceTemplateAbbreviation;
ALTER TABLE ServiceTemplates RENAME ServiceName TO ServiceTemplateName;
ALTER TABLE ServiceTemplates RENAME ServiceDescription TO ServiceTemplateDescription;

ALTER TABLE ServiceInstances RENAME TO Services;
ALTER TABLE Services RENAME ServiceInstanceName TO ServiceName;
ALTER TABLE Services RENAME ServiceInstanceAbbreviation TO ServiceAbbreviation;
ALTER TABLE ConnectTtSi RENAME ServiceInstanceId TO ServiceId;

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
	,ServiceTemplates.ServiceTemplateAbbreviation AS ServiceTemplateAbbreviation
	,ServiceTemplates.ServiceTemplateName AS ServiceTemplateName
	,TimeEvents.LocationId AS LocationId
	,TimeEvents.ServiceTemplateId AS ServiceTemplateId
FROM TimeEvents, Services, Locations
WHERE ServiceTemplates.Id= TimeEvents.ServiceTemplateId AND Locations.Id= TimeEvents.LocationId
ORDER BY ServiceTemplates.Id ASC, TimeEvents.[Order] ASC;
COMMIT;
