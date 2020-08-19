-- FUNCTION: public.create_geodesic_line(geometry, geometry)

-- DROP FUNCTION public.create_geodesic_line(geometry, geometry);

CREATE OR REPLACE FUNCTION public.appgis_create_geodesic_line(
	g1 geometry,
	g2 geometry)
    RETURNS geometry
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
AS $BODY$
DECLARE r geometry;
BEGIN 

  WITH lines AS (SELECT ST_Segmentize(
	ST_MakeLine(
		ST_SetSRID(g1,4326), 
		ST_SetSRID(g2,4326) 
	)::geography, 10000)::geometry as the_geom),
	tosplit AS (
  		SELECT * FROM lines
  		WHERE ST_XMax(the_geom) - ST_XMin(the_geom) > 180
	), 
	nosplit AS (
  		SELECT * FROM lines
  		WHERE ST_XMax(the_geom) - ST_XMin(the_geom) <= 180
	),
	split AS (
	  SELECT
		ST_Difference(ST_Shift_Longitude(the_geom),
					ST_Buffer(ST_GeomFromText('LINESTRING(180 90, 180 -90)',4326),
					0.00001)) AS the_geom
	  FROM tosplit
	),
	final AS (
	  SELECT * FROM split
	  UNION ALL
	  SELECT * FROM nosplit
	)
  	SELECT the_geom FROM final into r;
  	RETURN r;
  
  
END;
$BODY$;

ALTER FUNCTION public.appgis_create_geodesic_line(geometry, geometry)
    OWNER TO postgres;


-- FUNCTION: public.create_geodesic_line_coord(double precision, double precision, double precision, double precision)

-- DROP FUNCTION public.create_geodesic_line_coord(double precision, double precision, double precision, double precision);

CREATE OR REPLACE FUNCTION public.appgis_create_geodesic_line_coord(
	ax double precision,
	ay double precision,
	cx double precision,
	cy double precision)
    RETURNS geometry
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
AS $BODY$
DECLARE r geometry;
BEGIN 
	
	WITH lines AS (SELECT ST_Segmentize(
	ST_MakeLine(
		ST_SetSRID(ST_MakePoint(ax,ay),4326), 
		ST_SetSRID(ST_MakePoint(cx,cy),4326) 
	)::geography, 10000)::geometry as the_geom),
	tosplit AS (
  		SELECT * FROM lines
  		WHERE ST_XMax(the_geom) - ST_XMin(the_geom) > 180
	), 
	nosplit AS (
  		SELECT * FROM lines
  		WHERE ST_XMax(the_geom) - ST_XMin(the_geom) <= 180
	),
	split AS (
	  SELECT
		ST_Difference(ST_Shift_Longitude(the_geom),
					ST_Buffer(ST_GeomFromText('LINESTRING(180 90, 180 -90)',4326),
					0.00001)) AS the_geom
	  FROM tosplit
	),
	final AS (
	  SELECT * FROM split
	  UNION ALL
	  SELECT * FROM nosplit
	)
  	SELECT the_geom FROM final into r;
  	RETURN r;
  
END;
$BODY$;

ALTER FUNCTION public.appgis_create_geodesic_line_coord(double precision, double precision, double precision, double precision)
    OWNER TO postgres;


-- FUNCTION: public.create_geodesic_line_coords(double precision[])

-- DROP FUNCTION public.create_geodesic_line_coords(double precision[]);

CREATE OR REPLACE FUNCTION public.appgis_create_geodesic_line_coords(
	coords double precision[])
    RETURNS SETOF geometry 
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    ROWS 1000
AS $BODY$
DECLARE 
	sqltext text;
	r geometry[];
	in_clause ALIAS FOR $1;
	rec double precision[];
	rec2 double precision[];
BEGIN 
	CREATE TEMP TABLE IF NOT EXISTS temp_geomtable (geomid integer, geom geometry(POINT,4326));
	--sqltext := FORMAT('CREATE SEQUENCE IF NOT EXISTS %s.%s_id_seq', $1, $2);
	--EXECUTE sqltext;
	--sqltext := FORMAT('CREATE TABLE IF NOT EXISTS %s.%s (id integer NOT NULL DEFAULT nextval(''%s.%s''::regclass), geom geometry(LINESTRING,4326), CONSTRAINT %s_pkey PRIMARY KEY (id))', $1,$2,$1,$2,$2);
	--EXECUTE sqltext;	
	
	FOR rec IN array_lower(in_clause, 1) .. array_upper(in_clause, 1)
	LOOP
		FOR rec2 IN 1 .. 2
		LOOP
			insert into temp_geomtable (geomid, geom) values(rec, ST_SetSRID(ST_MakePoint(in_clause[rec][1],in_clause[rec][2]),4326));
		END LOOP;
		RAISE NOTICE '%', in_clause[rec][2];
	END LOOP; 
	return QUERY
																													  
	WITH lines AS (SELECT ST_Segmentize(
	ST_MakeLine(geom)::geography, 10000)::geometry as the_geom FROM temp_geomtable),
	tosplit AS (
  		SELECT * FROM lines
  		WHERE ST_XMax(the_geom) - ST_XMin(the_geom) > 180
	), 
	nosplit AS (
  		SELECT * FROM lines
  		WHERE ST_XMax(the_geom) - ST_XMin(the_geom) <= 180
	),
	split AS (
	  SELECT
		ST_Difference(ST_Shift_Longitude(the_geom),
					ST_Buffer(ST_GeomFromText('LINESTRING(180 90, 180 -90)',4326),
					0.00001)) AS the_geom
	  FROM tosplit
	),
	finalg AS (
	  SELECT * FROM split
	  UNION ALL
	  SELECT * FROM nosplit
	)																						  
	
	SELECT * FROM finalg;
	
	DROP TABLE temp_geomtable;
					  
					  
  	RETURN;
  
END;
$BODY$;

ALTER FUNCTION public.appgis_create_geodesic_line_coords(double precision[])
    OWNER TO postgres;



-- FUNCTION: public.create_geodesic_line_table(text, text, geometry)

-- DROP FUNCTION public.create_geodesic_line_table(text, text, geometry);

CREATE OR REPLACE FUNCTION public.appgis_create_geodesic_line_table(
	schemaname text,
	tablename text,
	geom geometry)
    RETURNS boolean
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
AS $BODY$
DECLARE 
	sqltext text;
	geomText text;
BEGIN 
	geomText := ST_AsText($3);
	sqltext := FORMAT('CREATE SEQUENCE IF NOT EXISTS %s.%s_id_seq', $1, $2);
	RAISE NOTICE '%', sqltext;
	EXECUTE sqltext;
	sqltext := FORMAT('CREATE TABLE IF NOT EXISTS %s.%s (id integer NOT NULL DEFAULT nextval(''%s.%s_id_seq''::regclass), geom geometry(LINESTRING,4326), CONSTRAINT %s_pkey PRIMARY KEY (id))', $1,$2,$1,$2,$2);
	RAISE NOTICE '%', sqltext;
	EXECUTE sqltext;	
	sqltext := FORMAT('INSERT INTO %s.%s (geom) values(ST_GeomFromText(''%s'',4326))', $1, $2, geomText);
	EXECUTE sqltext;
	RETURN 1;
																													  
END;
$BODY$;

ALTER FUNCTION public.appgis_create_geodesic_line_table(text, text, geometry)
    OWNER TO postgres;



CREATE OR REPLACE FUNCTION public.appgis_return_points_in_polygon(
	_points text,
	_polygon text,
	_in_format text default 'json')
    RETURNS table (document json)  
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    ROWS 1000
AS $BODY$
DECLARE 
	in_points ALIAS FOR $1;
	in_polygon ALIAS FOR $2;
	in_format ALIAS FOR $3;
BEGIN 
	
	IF in_format = 'json' THEN
		CREATE TEMP TABLE IF NOT EXISTS _point_features as SELECT json_array_elements(in_points::JSON -> 'features') as features;
		CREATE TEMP TABLE IF NOT EXISTS _polygon_feature as SELECT json_array_elements(in_polygon::JSON -> 'features') as features;
	ELSE
		RETURN;
	END IF;
	
	RETURN QUERY
	
	SELECT a.*
		FROM _point_features AS a
		join _polygon_feature AS b
			on (ST_WITHIN(ST_GeomFromGeoJSON((a.features -> 'geometry')::text), ST_GeomFromGeoJSON((b.features -> 'geometry')::text)));
	
	DROP TABLE _point_features;
	DROP TABLE _polygon_feature;
					  
  
END;
$BODY$;

ALTER FUNCTION public.appgis_return_points_in_polygon(_points text, _polygon text, _in_format text)
    OWNER TO postgres;