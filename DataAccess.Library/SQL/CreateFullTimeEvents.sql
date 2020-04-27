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