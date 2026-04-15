--
-- PostgreSQL database cluster dump
--

-- Started on 2026-03-27 18:25:01

\restrict VQfroPwDkRlsSoNj3QuXgeXXofYMxYdnetmdYII8aNhcQdoh9BTYI92Z84rjnae

SET default_transaction_read_only = off;

SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;

--
-- Roles
--

CREATE ROLE postgres;
ALTER ROLE postgres WITH SUPERUSER INHERIT CREATEROLE CREATEDB LOGIN REPLICATION BYPASSRLS PASSWORD 'SCRAM-SHA-256$4096:1EGbs9YgxlorO4WPM0j6LQ==$rtYUGtX82PEbpqUKnW9vD0h92ltuLnkkVuHQmVhYtBM=:KHZ3UuvpcJs9cyiPLYLXhBh8u8HwM9WDwOtenSbOiFU=';

--
-- User Configurations
--








\unrestrict VQfroPwDkRlsSoNj3QuXgeXXofYMxYdnetmdYII8aNhcQdoh9BTYI92Z84rjnae

--
-- Databases
--

--
-- Database "template1" dump
--

\connect template1

--
-- PostgreSQL database dump
--

\restrict rj4P97Ma9qX0885UPZQnwodcwxfwlDDeZ83UI4Ze3D9HAZN3kyrnQ5wsJQXtomu

-- Dumped from database version 18.3
-- Dumped by pg_dump version 18.3

-- Started on 2026-03-27 18:25:01

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

-- Completed on 2026-03-27 18:25:02

--
-- PostgreSQL database dump complete
--

\unrestrict rj4P97Ma9qX0885UPZQnwodcwxfwlDDeZ83UI4Ze3D9HAZN3kyrnQ5wsJQXtomu

--
-- Database "FinanceTrackerLocalDb" dump
--

--
-- PostgreSQL database dump
--

\restrict gdp14yHr3Q2lXsPChVfgmWp7I2ZnHqYOkQyeX5D29wQuTodNEUMlNeHjpDhbE13

-- Dumped from database version 18.3
-- Dumped by pg_dump version 18.3

-- Started on 2026-03-27 18:25:02

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 5207 (class 1262 OID 16397)
-- Name: FinanceTrackerLocalDb; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE "FinanceTrackerLocalDb" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_Australia.1256';


ALTER DATABASE "FinanceTrackerLocalDb" OWNER TO postgres;

\unrestrict gdp14yHr3Q2lXsPChVfgmWp7I2ZnHqYOkQyeX5D29wQuTodNEUMlNeHjpDhbE13
\connect "FinanceTrackerLocalDb"
\restrict gdp14yHr3Q2lXsPChVfgmWp7I2ZnHqYOkQyeX5D29wQuTodNEUMlNeHjpDhbE13

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 258 (class 1255 OID 16745)
-- Name: usp_getalllogs(integer, integer, integer, integer, integer, character varying, character varying, date); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.usp_getalllogs(p_page integer DEFAULT 1, p_pagesize integer DEFAULT 50, p_userid integer DEFAULT NULL::integer, p_statuscode integer DEFAULT NULL::integer, p_securitylevel integer DEFAULT NULL::integer, p_httpmethod character varying DEFAULT NULL::character varying, p_ip character varying DEFAULT NULL::character varying, p_date date DEFAULT NULL::date) RETURNS TABLE(logid integer, userid integer, action character varying, httpmethod character varying, endpoint character varying, ipaddress character varying, createddate date, createdhour time without time zone, statuscode text, securitylevel text, totalrows bigint)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT 
        l.LogId, l.UserId, l.Action, l.HttpMethod, l.Endpoint,
        l.IpAddress,
        CAST(l.CreatedAt AS DATE) AS CreatedDate,
        CAST(l.CreatedAt AS TIME) AS CreatedHour,
        CASE l.StatusCode
            WHEN 200 THEN '200 - OK'
            WHEN 201 THEN '201 - Created'
            WHEN 400 THEN '400 - Bad Request'
            WHEN 401 THEN '401 - Unauthorized'
            WHEN 403 THEN '403 - Forbidden'
            WHEN 404 THEN '404 - Not Found'
            WHEN 429 THEN '429 - Too Many Requests'
            WHEN 500 THEN '500 - Internal Server Error'
            ELSE CAST(l.StatusCode AS TEXT) || ' - Unknown'
        END AS StatusCode,
        CASE l.SecurityLevel
            WHEN 1 THEN 'Low'
            WHEN 2 THEN 'Medium'
            WHEN 3 THEN 'Dangerous'
            ELSE 'Unknown'
        END AS SecurityLevel,
        COUNT(*) OVER() AS TotalRows
    FROM Logs l
    WHERE (p_UserId IS NULL OR l.UserId = p_UserId)
    AND (p_StatusCode IS NULL OR l.StatusCode = p_StatusCode)
    AND (p_SecurityLevel IS NULL OR l.SecurityLevel = p_SecurityLevel)
    AND (p_HttpMethod IS NULL OR l.HttpMethod = p_HttpMethod)
    AND (p_Ip IS NULL OR l.IpAddress LIKE '%' || p_Ip || '%')
    AND (p_Date IS NULL OR CAST(l.CreatedAt AS DATE) = p_Date)
    ORDER BY l.CreatedAt DESC
    LIMIT p_PageSize OFFSET (p_Page - 1) * p_PageSize;
END;
$$;


ALTER FUNCTION public.usp_getalllogs(p_page integer, p_pagesize integer, p_userid integer, p_statuscode integer, p_securitylevel integer, p_httpmethod character varying, p_ip character varying, p_date date) OWNER TO postgres;

--
-- TOC entry 261 (class 1255 OID 16748)
-- Name: usp_getapihitscount(date, date, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.usp_getapihitscount(p_startdate date DEFAULT NULL::date, p_enddate date DEFAULT NULL::date, p_userid integer DEFAULT NULL::integer) RETURNS TABLE(action character varying, hitscount bigint)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT l.Action, COUNT(*) AS HitsCount
    FROM Logs l
    WHERE l.CreatedAt BETWEEN
        COALESCE(p_StartDate, CURRENT_DATE - INTERVAL '6 months')
        AND COALESCE(p_EndDate, CURRENT_DATE)
    AND (p_UserId IS NULL OR l.UserId = p_UserId)
    GROUP BY l.Action;
END;
$$;


ALTER FUNCTION public.usp_getapihitscount(p_startdate date, p_enddate date, p_userid integer) OWNER TO postgres;

--
-- TOC entry 259 (class 1255 OID 16746)
-- Name: usp_getspamrequests(smallint, date, smallint, character varying, integer, character varying, integer, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.usp_getspamrequests(p_statuscode smallint DEFAULT NULL::smallint, p_date date DEFAULT NULL::date, p_securitylevel smallint DEFAULT 3, p_httpmethod character varying DEFAULT 'POST'::character varying, p_userid integer DEFAULT NULL::integer, p_ip character varying DEFAULT NULL::character varying, p_page integer DEFAULT 1, p_pagesize integer DEFAULT 50) RETURNS TABLE(logid integer, userid integer, action character varying, httpmethod character varying, endpoint character varying, ipaddress character varying, createddate date, hitscount bigint, statuscode text, securitylevel text, totalrows bigint)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT 
        s.LogId, s.UserId, s.Action, s.HttpMethod, s.Endpoint,
        s.IpAddress, s.CreatedDate, s.HitsCount, s.StatusCode,
        s.SecurityLevel, COUNT(*) OVER() AS TotalRows
    FROM (
        SELECT
            l.LogId, l.UserId, l.Action, l.HttpMethod, l.Endpoint,
            l.IpAddress,
            CAST(l.CreatedAt AS DATE) AS CreatedDate,
            ROW_NUMBER() OVER (PARTITION BY l.IpAddress, l.Endpoint, CAST(l.CreatedAt AS DATE) ORDER BY l.CreatedAt DESC) AS RowNum,
            COUNT(*) OVER (PARTITION BY l.IpAddress, l.Endpoint, CAST(l.CreatedAt AS DATE)) AS HitsCount,
            CASE l.StatusCode
                WHEN 200 THEN '200 - OK'
                WHEN 201 THEN '201 - Created'
                WHEN 400 THEN '400 - Bad Request'
                WHEN 401 THEN '401 - Unauthorized'
                WHEN 403 THEN '403 - Forbidden'
                WHEN 404 THEN '404 - Not Found'
                WHEN 429 THEN '429 - Too Many Requests'
                WHEN 500 THEN '500 - Internal Server Error'
                ELSE CAST(l.StatusCode AS TEXT) || ' - Unknown'
            END AS StatusCode,
            CASE l.SecurityLevel
                WHEN 1 THEN 'Low'
                WHEN 2 THEN 'Medium'
                WHEN 3 THEN 'Dangerous'
                ELSE 'Unknown'
            END AS SecurityLevel
        FROM Logs l
        WHERE (p_StatusCode IS NULL OR l.StatusCode = p_StatusCode)
        AND l.SecurityLevel = p_SecurityLevel
        AND CAST(l.CreatedAt AS DATE) BETWEEN
            COALESCE(p_Date, CURRENT_DATE - INTERVAL '1 month')
            AND COALESCE(p_Date, CURRENT_DATE)
        AND (p_UserId IS NULL OR l.UserId = p_UserId)
        AND (p_Ip IS NULL OR l.IpAddress LIKE '%' || p_Ip || '%')
    ) s
    WHERE s.RowNum = 1
    AND s.HitsCount > 4
    ORDER BY s.HitsCount DESC
    LIMIT p_PageSize OFFSET (p_Page - 1) * p_PageSize;
END;
$$;


ALTER FUNCTION public.usp_getspamrequests(p_statuscode smallint, p_date date, p_securitylevel smallint, p_httpmethod character varying, p_userid integer, p_ip character varying, p_page integer, p_pagesize integer) OWNER TO postgres;

--
-- TOC entry 260 (class 1255 OID 16747)
-- Name: usp_getstatushitscount(date, date, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.usp_getstatushitscount(p_startdate date DEFAULT NULL::date, p_enddate date DEFAULT NULL::date, p_userid integer DEFAULT NULL::integer) RETURNS TABLE(statuscode smallint, hitscount bigint)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT l.StatusCode, COUNT(*) AS HitsCount
    FROM Logs l
    WHERE l.CreatedAt BETWEEN
        COALESCE(p_StartDate, CURRENT_DATE - INTERVAL '6 months')
        AND COALESCE(p_EndDate, CURRENT_DATE)
    AND (p_UserId IS NULL OR l.UserId = p_UserId)
    GROUP BY l.StatusCode;
END;
$$;


ALTER FUNCTION public.usp_getstatushitscount(p_startdate date, p_enddate date, p_userid integer) OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 226 (class 1259 OID 16448)
-- Name: blockedusers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.blockedusers (
    blockeduserid integer NOT NULL,
    userid integer NOT NULL,
    targetuserid integer NOT NULL,
    createdat date DEFAULT CURRENT_DATE NOT NULL
);


ALTER TABLE public.blockedusers OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 16447)
-- Name: blockedusers_blockeduserid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.blockedusers_blockeduserid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.blockedusers_blockeduserid_seq OWNER TO postgres;

--
-- TOC entry 5208 (class 0 OID 0)
-- Dependencies: 225
-- Name: blockedusers_blockeduserid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.blockedusers_blockeduserid_seq OWNED BY public.blockedusers.blockeduserid;


--
-- TOC entry 228 (class 1259 OID 16472)
-- Name: bugreports; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.bugreports (
    bugreportid integer NOT NULL,
    userid integer NOT NULL,
    description character varying(500) NOT NULL,
    status boolean DEFAULT false
);


ALTER TABLE public.bugreports OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 16471)
-- Name: bugreports_bugreportid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.bugreports_bugreportid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.bugreports_bugreportid_seq OWNER TO postgres;

--
-- TOC entry 5209 (class 0 OID 0)
-- Dependencies: 227
-- Name: bugreports_bugreportid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.bugreports_bugreportid_seq OWNED BY public.bugreports.bugreportid;


--
-- TOC entry 230 (class 1259 OID 16490)
-- Name: categories; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.categories (
    categoryid integer NOT NULL,
    categoryname character varying(70) NOT NULL,
    colorhex character(7) DEFAULT '#000000'::bpchar NOT NULL,
    iconname character varying(50),
    walletid integer NOT NULL,
    isactive boolean DEFAULT true NOT NULL
);


ALTER TABLE public.categories OWNER TO postgres;

--
-- TOC entry 229 (class 1259 OID 16489)
-- Name: categories_categoryid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.categories_categoryid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.categories_categoryid_seq OWNER TO postgres;

--
-- TOC entry 5210 (class 0 OID 0)
-- Dependencies: 229
-- Name: categories_categoryid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.categories_categoryid_seq OWNED BY public.categories.categoryid;


--
-- TOC entry 232 (class 1259 OID 16511)
-- Name: chatmessages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.chatmessages (
    messageid integer NOT NULL,
    senderuserid integer NOT NULL,
    receiveruserid integer NOT NULL,
    content character varying(1000) NOT NULL,
    createdat timestamp with time zone DEFAULT now() NOT NULL,
    isread boolean DEFAULT false NOT NULL
);


ALTER TABLE public.chatmessages OWNER TO postgres;

--
-- TOC entry 231 (class 1259 OID 16510)
-- Name: chatmessages_messageid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.chatmessages_messageid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.chatmessages_messageid_seq OWNER TO postgres;

--
-- TOC entry 5211 (class 0 OID 0)
-- Dependencies: 231
-- Name: chatmessages_messageid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.chatmessages_messageid_seq OWNED BY public.chatmessages.messageid;


--
-- TOC entry 234 (class 1259 OID 16538)
-- Name: expenses; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.expenses (
    expenseid integer NOT NULL,
    userid integer NOT NULL,
    walletid integer NOT NULL,
    categoryid integer NOT NULL,
    amount numeric(18,2) NOT NULL,
    description character varying(300),
    createdat date DEFAULT CURRENT_DATE NOT NULL
);


ALTER TABLE public.expenses OWNER TO postgres;

--
-- TOC entry 233 (class 1259 OID 16537)
-- Name: expenses_expenseid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.expenses_expenseid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.expenses_expenseid_seq OWNER TO postgres;

--
-- TOC entry 5212 (class 0 OID 0)
-- Dependencies: 233
-- Name: expenses_expenseid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.expenses_expenseid_seq OWNED BY public.expenses.expenseid;


--
-- TOC entry 236 (class 1259 OID 16567)
-- Name: friendships; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.friendships (
    friendshipid integer NOT NULL,
    userid integer NOT NULL,
    frienduserid integer NOT NULL,
    status smallint DEFAULT 0 NOT NULL,
    createdat date DEFAULT CURRENT_DATE NOT NULL
);


ALTER TABLE public.friendships OWNER TO postgres;

--
-- TOC entry 235 (class 1259 OID 16566)
-- Name: friendships_friendshipid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.friendships_friendshipid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.friendships_friendshipid_seq OWNER TO postgres;

--
-- TOC entry 5213 (class 0 OID 0)
-- Dependencies: 235
-- Name: friendships_friendshipid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.friendships_friendshipid_seq OWNED BY public.friendships.friendshipid;


--
-- TOC entry 240 (class 1259 OID 16610)
-- Name: income; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.income (
    incomeid integer NOT NULL,
    amount numeric(18,2) NOT NULL,
    walletid integer NOT NULL,
    userid integer NOT NULL,
    incomesourceid integer NOT NULL,
    description character varying(300),
    createdat date DEFAULT CURRENT_DATE NOT NULL,
    CONSTRAINT income_amount_check CHECK ((amount > (0)::numeric))
);


ALTER TABLE public.income OWNER TO postgres;

--
-- TOC entry 239 (class 1259 OID 16609)
-- Name: income_incomeid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.income_incomeid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.income_incomeid_seq OWNER TO postgres;

--
-- TOC entry 5214 (class 0 OID 0)
-- Dependencies: 239
-- Name: income_incomeid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.income_incomeid_seq OWNED BY public.income.incomeid;


--
-- TOC entry 238 (class 1259 OID 16593)
-- Name: incomesources; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.incomesources (
    incomesourceid integer NOT NULL,
    incomesourcename character varying(50) NOT NULL,
    isactive boolean DEFAULT true NOT NULL,
    walletid integer NOT NULL
);


ALTER TABLE public.incomesources OWNER TO postgres;

--
-- TOC entry 237 (class 1259 OID 16592)
-- Name: incomesources_incomesourceid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.incomesources_incomesourceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.incomesources_incomesourceid_seq OWNER TO postgres;

--
-- TOC entry 5215 (class 0 OID 0)
-- Dependencies: 237
-- Name: incomesources_incomesourceid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.incomesources_incomesourceid_seq OWNED BY public.incomesources.incomesourceid;


--
-- TOC entry 242 (class 1259 OID 16640)
-- Name: logs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.logs (
    logid integer NOT NULL,
    userid integer,
    action character varying(100) NOT NULL,
    httpmethod character varying(10) NOT NULL,
    endpoint character varying(255) NOT NULL,
    statuscode smallint NOT NULL,
    securitylevel smallint NOT NULL,
    ipaddress character varying(45) NOT NULL,
    createdat timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE public.logs OWNER TO postgres;

--
-- TOC entry 241 (class 1259 OID 16639)
-- Name: logs_logid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.logs_logid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.logs_logid_seq OWNER TO postgres;

--
-- TOC entry 5216 (class 0 OID 0)
-- Dependencies: 241
-- Name: logs_logid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.logs_logid_seq OWNED BY public.logs.logid;


--
-- TOC entry 220 (class 1259 OID 16399)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    userid integer NOT NULL,
    username character varying(50) NOT NULL,
    email character varying(254) NOT NULL,
    passwordhash character varying(255) NOT NULL,
    isactive boolean DEFAULT true NOT NULL,
    createdat date DEFAULT CURRENT_DATE NOT NULL,
    emailverificationtoken character varying(255),
    emailverificationexpiresat timestamp with time zone,
    emailverified boolean DEFAULT false NOT NULL,
    refreshtokenhash character varying(255),
    refreshtokenexpiresat timestamp with time zone,
    refreshtokenrevokedat timestamp with time zone
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 16398)
-- Name: users_userid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_userid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.users_userid_seq OWNER TO postgres;

--
-- TOC entry 5217 (class 0 OID 0)
-- Dependencies: 219
-- Name: users_userid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_userid_seq OWNED BY public.users.userid;


--
-- TOC entry 244 (class 1259 OID 16661)
-- Name: userwallets; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.userwallets (
    userwalletid integer NOT NULL,
    userid integer NOT NULL,
    walletid integer NOT NULL,
    walletroleid smallint DEFAULT 1 NOT NULL
);


ALTER TABLE public.userwallets OWNER TO postgres;

--
-- TOC entry 243 (class 1259 OID 16660)
-- Name: userwallets_userwalletid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.userwallets_userwalletid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.userwallets_userwalletid_seq OWNER TO postgres;

--
-- TOC entry 5218 (class 0 OID 0)
-- Dependencies: 243
-- Name: userwallets_userwalletid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.userwallets_userwalletid_seq OWNED BY public.userwallets.userwalletid;


--
-- TOC entry 246 (class 1259 OID 16690)
-- Name: walletgoals; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.walletgoals (
    walletgoalid integer NOT NULL,
    walletid integer NOT NULL,
    goalname character varying(50) NOT NULL,
    description character varying(500),
    allocatedamount numeric(18,2) DEFAULT 0 NOT NULL,
    targetamount numeric(18,2) NOT NULL,
    targetdate date,
    iscompleted boolean DEFAULT false NOT NULL,
    priority smallint DEFAULT 1 NOT NULL,
    goalimageurl character varying(500),
    createdat date DEFAULT CURRENT_DATE NOT NULL,
    CONSTRAINT ck_walletgoals_allocated CHECK ((allocatedamount <= targetamount)),
    CONSTRAINT walletgoals_priority_check CHECK (((priority >= 1) AND (priority <= 3))),
    CONSTRAINT walletgoals_targetamount_check CHECK ((targetamount > (0)::numeric))
);


ALTER TABLE public.walletgoals OWNER TO postgres;

--
-- TOC entry 245 (class 1259 OID 16689)
-- Name: walletgoals_walletgoalid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.walletgoals_walletgoalid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.walletgoals_walletgoalid_seq OWNER TO postgres;

--
-- TOC entry 5219 (class 0 OID 0)
-- Dependencies: 245
-- Name: walletgoals_walletgoalid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.walletgoals_walletgoalid_seq OWNED BY public.walletgoals.walletgoalid;


--
-- TOC entry 224 (class 1259 OID 16437)
-- Name: walletroles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.walletroles (
    walletroleid smallint NOT NULL,
    rolename character varying(50) NOT NULL
);


ALTER TABLE public.walletroles OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 16436)
-- Name: walletroles_walletroleid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.walletroles_walletroleid_seq
    AS smallint
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.walletroles_walletroleid_seq OWNER TO postgres;

--
-- TOC entry 5220 (class 0 OID 0)
-- Dependencies: 223
-- Name: walletroles_walletroleid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.walletroles_walletroleid_seq OWNED BY public.walletroles.walletroleid;


--
-- TOC entry 222 (class 1259 OID 16422)
-- Name: wallets; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.wallets (
    walletid integer NOT NULL,
    walletname character varying(50) NOT NULL,
    savedbalance numeric(18,2) DEFAULT 0 NOT NULL,
    availablebalance numeric(18,2) DEFAULT 0 NOT NULL,
    createdat date DEFAULT CURRENT_DATE NOT NULL
);


ALTER TABLE public.wallets OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 16421)
-- Name: wallets_walletid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.wallets_walletid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.wallets_walletid_seq OWNER TO postgres;

--
-- TOC entry 5221 (class 0 OID 0)
-- Dependencies: 221
-- Name: wallets_walletid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.wallets_walletid_seq OWNED BY public.wallets.walletid;


--
-- TOC entry 4934 (class 2604 OID 16451)
-- Name: blockedusers blockeduserid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.blockedusers ALTER COLUMN blockeduserid SET DEFAULT nextval('public.blockedusers_blockeduserid_seq'::regclass);


--
-- TOC entry 4936 (class 2604 OID 16475)
-- Name: bugreports bugreportid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bugreports ALTER COLUMN bugreportid SET DEFAULT nextval('public.bugreports_bugreportid_seq'::regclass);


--
-- TOC entry 4938 (class 2604 OID 16493)
-- Name: categories categoryid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categories ALTER COLUMN categoryid SET DEFAULT nextval('public.categories_categoryid_seq'::regclass);


--
-- TOC entry 4941 (class 2604 OID 16514)
-- Name: chatmessages messageid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.chatmessages ALTER COLUMN messageid SET DEFAULT nextval('public.chatmessages_messageid_seq'::regclass);


--
-- TOC entry 4944 (class 2604 OID 16541)
-- Name: expenses expenseid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.expenses ALTER COLUMN expenseid SET DEFAULT nextval('public.expenses_expenseid_seq'::regclass);


--
-- TOC entry 4946 (class 2604 OID 16570)
-- Name: friendships friendshipid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.friendships ALTER COLUMN friendshipid SET DEFAULT nextval('public.friendships_friendshipid_seq'::regclass);


--
-- TOC entry 4951 (class 2604 OID 16613)
-- Name: income incomeid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.income ALTER COLUMN incomeid SET DEFAULT nextval('public.income_incomeid_seq'::regclass);


--
-- TOC entry 4949 (class 2604 OID 16596)
-- Name: incomesources incomesourceid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.incomesources ALTER COLUMN incomesourceid SET DEFAULT nextval('public.incomesources_incomesourceid_seq'::regclass);


--
-- TOC entry 4953 (class 2604 OID 16643)
-- Name: logs logid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.logs ALTER COLUMN logid SET DEFAULT nextval('public.logs_logid_seq'::regclass);


--
-- TOC entry 4925 (class 2604 OID 16402)
-- Name: users userid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN userid SET DEFAULT nextval('public.users_userid_seq'::regclass);


--
-- TOC entry 4955 (class 2604 OID 16664)
-- Name: userwallets userwalletid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.userwallets ALTER COLUMN userwalletid SET DEFAULT nextval('public.userwallets_userwalletid_seq'::regclass);


--
-- TOC entry 4957 (class 2604 OID 16693)
-- Name: walletgoals walletgoalid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.walletgoals ALTER COLUMN walletgoalid SET DEFAULT nextval('public.walletgoals_walletgoalid_seq'::regclass);


--
-- TOC entry 4933 (class 2604 OID 16440)
-- Name: walletroles walletroleid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.walletroles ALTER COLUMN walletroleid SET DEFAULT nextval('public.walletroles_walletroleid_seq'::regclass);


--
-- TOC entry 4929 (class 2604 OID 16425)
-- Name: wallets walletid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wallets ALTER COLUMN walletid SET DEFAULT nextval('public.wallets_walletid_seq'::regclass);


--
-- TOC entry 4979 (class 2606 OID 16458)
-- Name: blockedusers blockedusers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.blockedusers
    ADD CONSTRAINT blockedusers_pkey PRIMARY KEY (blockeduserid);


--
-- TOC entry 4985 (class 2606 OID 16483)
-- Name: bugreports bugreports_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bugreports
    ADD CONSTRAINT bugreports_pkey PRIMARY KEY (bugreportid);


--
-- TOC entry 4988 (class 2606 OID 16502)
-- Name: categories categories_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categories
    ADD CONSTRAINT categories_pkey PRIMARY KEY (categoryid);


--
-- TOC entry 4993 (class 2606 OID 16526)
-- Name: chatmessages chatmessages_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.chatmessages
    ADD CONSTRAINT chatmessages_pkey PRIMARY KEY (messageid);


--
-- TOC entry 4997 (class 2606 OID 16550)
-- Name: expenses expenses_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.expenses
    ADD CONSTRAINT expenses_pkey PRIMARY KEY (expenseid);


--
-- TOC entry 5001 (class 2606 OID 16579)
-- Name: friendships friendships_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.friendships
    ADD CONSTRAINT friendships_pkey PRIMARY KEY (friendshipid);


--
-- TOC entry 5012 (class 2606 OID 16623)
-- Name: income income_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.income
    ADD CONSTRAINT income_pkey PRIMARY KEY (incomeid);


--
-- TOC entry 5008 (class 2606 OID 16603)
-- Name: incomesources incomesources_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.incomesources
    ADD CONSTRAINT incomesources_pkey PRIMARY KEY (incomesourceid);


--
-- TOC entry 5024 (class 2606 OID 16654)
-- Name: logs logs_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.logs
    ADD CONSTRAINT logs_pkey PRIMARY KEY (logid);


--
-- TOC entry 4983 (class 2606 OID 16460)
-- Name: blockedusers uq_blockedusers_userblocked; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.blockedusers
    ADD CONSTRAINT uq_blockedusers_userblocked UNIQUE (userid, targetuserid);


--
-- TOC entry 4991 (class 2606 OID 16504)
-- Name: categories uq_categories_walletname; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categories
    ADD CONSTRAINT uq_categories_walletname UNIQUE (walletid, categoryname);


--
-- TOC entry 5006 (class 2606 OID 16581)
-- Name: friendships uq_friendships; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.friendships
    ADD CONSTRAINT uq_friendships UNIQUE (userid, frienduserid);


--
-- TOC entry 5029 (class 2606 OID 16673)
-- Name: userwallets uq_userwallets; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.userwallets
    ADD CONSTRAINT uq_userwallets UNIQUE (userid, walletid);


--
-- TOC entry 4967 (class 2606 OID 16420)
-- Name: users users_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_email_key UNIQUE (email);


--
-- TOC entry 4969 (class 2606 OID 16416)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (userid);


--
-- TOC entry 4971 (class 2606 OID 16418)
-- Name: users users_username_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_username_key UNIQUE (username);


--
-- TOC entry 5031 (class 2606 OID 16671)
-- Name: userwallets userwallets_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.userwallets
    ADD CONSTRAINT userwallets_pkey PRIMARY KEY (userwalletid);


--
-- TOC entry 5034 (class 2606 OID 16712)
-- Name: walletgoals walletgoals_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.walletgoals
    ADD CONSTRAINT walletgoals_pkey PRIMARY KEY (walletgoalid);


--
-- TOC entry 4975 (class 2606 OID 16444)
-- Name: walletroles walletroles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.walletroles
    ADD CONSTRAINT walletroles_pkey PRIMARY KEY (walletroleid);


--
-- TOC entry 4977 (class 2606 OID 16446)
-- Name: walletroles walletroles_rolename_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.walletroles
    ADD CONSTRAINT walletroles_rolename_key UNIQUE (rolename);


--
-- TOC entry 4973 (class 2606 OID 16435)
-- Name: wallets wallets_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wallets
    ADD CONSTRAINT wallets_pkey PRIMARY KEY (walletid);


--
-- TOC entry 4980 (class 1259 OID 16719)
-- Name: ix_blockedusers_targetuserid; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_blockedusers_targetuserid ON public.blockedusers USING btree (targetuserid);


--
-- TOC entry 4981 (class 1259 OID 16718)
-- Name: ix_blockedusers_userid; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_blockedusers_userid ON public.blockedusers USING btree (userid);


--
-- TOC entry 4986 (class 1259 OID 16720)
-- Name: ix_bugreports_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_bugreports_status ON public.bugreports USING btree (status);


--
-- TOC entry 4989 (class 1259 OID 16721)
-- Name: ix_categories_walletid; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_categories_walletid ON public.categories USING btree (walletid);


--
-- TOC entry 4994 (class 1259 OID 16794)
-- Name: ix_chatmessages_conversation; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_chatmessages_conversation ON public.chatmessages USING btree (senderuserid, receiveruserid, createdat);


--
-- TOC entry 4995 (class 1259 OID 16723)
-- Name: ix_chatmessages_isread; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_chatmessages_isread ON public.chatmessages USING btree (isread);


--
-- TOC entry 4998 (class 1259 OID 16724)
-- Name: ix_expenses_wallet_category; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_expenses_wallet_category ON public.expenses USING btree (walletid, categoryid, userid);


--
-- TOC entry 4999 (class 1259 OID 16725)
-- Name: ix_expenses_wallet_user; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_expenses_wallet_user ON public.expenses USING btree (walletid, userid);


--
-- TOC entry 5002 (class 1259 OID 16726)
-- Name: ix_friends_frienduserid; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_friends_frienduserid ON public.friendships USING btree (frienduserid);


--
-- TOC entry 5003 (class 1259 OID 16727)
-- Name: ix_friends_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_friends_status ON public.friendships USING btree (status);


--
-- TOC entry 5004 (class 1259 OID 16728)
-- Name: ix_friends_userid_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_friends_userid_status ON public.friendships USING btree (userid, status);


--
-- TOC entry 5013 (class 1259 OID 16729)
-- Name: ix_income_incomesourceid; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_income_incomesourceid ON public.income USING btree (incomesourceid);


--
-- TOC entry 5014 (class 1259 OID 16730)
-- Name: ix_income_user; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_income_user ON public.income USING btree (userid);


--
-- TOC entry 5015 (class 1259 OID 16731)
-- Name: ix_income_wallet_user; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_income_wallet_user ON public.income USING btree (walletid, userid);


--
-- TOC entry 5009 (class 1259 OID 16732)
-- Name: ix_incomesource_incomesourceid; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_incomesource_incomesourceid ON public.incomesources USING btree (incomesourceid);


--
-- TOC entry 5010 (class 1259 OID 16733)
-- Name: ix_incomesources_walletid; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_incomesources_walletid ON public.incomesources USING btree (walletid);


--
-- TOC entry 5016 (class 1259 OID 16832)
-- Name: ix_logs_createdat; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_logs_createdat ON public.logs USING btree (createdat);


--
-- TOC entry 5017 (class 1259 OID 16735)
-- Name: ix_logs_endpoint_method; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_logs_endpoint_method ON public.logs USING btree (endpoint, httpmethod);


--
-- TOC entry 5018 (class 1259 OID 16736)
-- Name: ix_logs_endpoint_statuscode; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_logs_endpoint_statuscode ON public.logs USING btree (endpoint, statuscode);


--
-- TOC entry 5019 (class 1259 OID 16737)
-- Name: ix_logs_securitylevel; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_logs_securitylevel ON public.logs USING btree (securitylevel);


--
-- TOC entry 5020 (class 1259 OID 16738)
-- Name: ix_logs_statuscode; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_logs_statuscode ON public.logs USING btree (statuscode);


--
-- TOC entry 5021 (class 1259 OID 16739)
-- Name: ix_logs_userid; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_logs_userid ON public.logs USING btree (userid);


--
-- TOC entry 5022 (class 1259 OID 16740)
-- Name: ix_logs_userid_action; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_logs_userid_action ON public.logs USING btree (userid, action);


--
-- TOC entry 5025 (class 1259 OID 16741)
-- Name: ix_userwallets_userid; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_userwallets_userid ON public.userwallets USING btree (userid);


--
-- TOC entry 5026 (class 1259 OID 16742)
-- Name: ix_userwallets_walletid; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_userwallets_walletid ON public.userwallets USING btree (walletid);


--
-- TOC entry 5027 (class 1259 OID 16743)
-- Name: ix_userwallets_walletroleid; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_userwallets_walletroleid ON public.userwallets USING btree (walletroleid);


--
-- TOC entry 5032 (class 1259 OID 16744)
-- Name: ix_walletgoals_walletid; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_walletgoals_walletid ON public.walletgoals USING btree (walletid);


--
-- TOC entry 5035 (class 2606 OID 16466)
-- Name: blockedusers blockedusers_targetuserid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.blockedusers
    ADD CONSTRAINT blockedusers_targetuserid_fkey FOREIGN KEY (targetuserid) REFERENCES public.users(userid);


--
-- TOC entry 5036 (class 2606 OID 16461)
-- Name: blockedusers blockedusers_userid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.blockedusers
    ADD CONSTRAINT blockedusers_userid_fkey FOREIGN KEY (userid) REFERENCES public.users(userid);


--
-- TOC entry 5037 (class 2606 OID 16484)
-- Name: bugreports bugreports_userid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bugreports
    ADD CONSTRAINT bugreports_userid_fkey FOREIGN KEY (userid) REFERENCES public.users(userid);


--
-- TOC entry 5038 (class 2606 OID 16505)
-- Name: categories categories_walletid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categories
    ADD CONSTRAINT categories_walletid_fkey FOREIGN KEY (walletid) REFERENCES public.wallets(walletid);


--
-- TOC entry 5039 (class 2606 OID 16532)
-- Name: chatmessages chatmessages_receiveruserid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.chatmessages
    ADD CONSTRAINT chatmessages_receiveruserid_fkey FOREIGN KEY (receiveruserid) REFERENCES public.users(userid);


--
-- TOC entry 5040 (class 2606 OID 16527)
-- Name: chatmessages chatmessages_senderuserid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.chatmessages
    ADD CONSTRAINT chatmessages_senderuserid_fkey FOREIGN KEY (senderuserid) REFERENCES public.users(userid);


--
-- TOC entry 5041 (class 2606 OID 16561)
-- Name: expenses expenses_categoryid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.expenses
    ADD CONSTRAINT expenses_categoryid_fkey FOREIGN KEY (categoryid) REFERENCES public.categories(categoryid);


--
-- TOC entry 5042 (class 2606 OID 16551)
-- Name: expenses expenses_userid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.expenses
    ADD CONSTRAINT expenses_userid_fkey FOREIGN KEY (userid) REFERENCES public.users(userid);


--
-- TOC entry 5043 (class 2606 OID 16556)
-- Name: expenses expenses_walletid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.expenses
    ADD CONSTRAINT expenses_walletid_fkey FOREIGN KEY (walletid) REFERENCES public.wallets(walletid);


--
-- TOC entry 5044 (class 2606 OID 16587)
-- Name: friendships friendships_frienduserid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.friendships
    ADD CONSTRAINT friendships_frienduserid_fkey FOREIGN KEY (frienduserid) REFERENCES public.users(userid);


--
-- TOC entry 5045 (class 2606 OID 16582)
-- Name: friendships friendships_userid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.friendships
    ADD CONSTRAINT friendships_userid_fkey FOREIGN KEY (userid) REFERENCES public.users(userid);


--
-- TOC entry 5047 (class 2606 OID 16634)
-- Name: income income_incomesourceid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.income
    ADD CONSTRAINT income_incomesourceid_fkey FOREIGN KEY (incomesourceid) REFERENCES public.incomesources(incomesourceid);


--
-- TOC entry 5048 (class 2606 OID 16629)
-- Name: income income_userid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.income
    ADD CONSTRAINT income_userid_fkey FOREIGN KEY (userid) REFERENCES public.users(userid);


--
-- TOC entry 5049 (class 2606 OID 16624)
-- Name: income income_walletid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.income
    ADD CONSTRAINT income_walletid_fkey FOREIGN KEY (walletid) REFERENCES public.wallets(walletid);


--
-- TOC entry 5046 (class 2606 OID 16604)
-- Name: incomesources incomesources_walletid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.incomesources
    ADD CONSTRAINT incomesources_walletid_fkey FOREIGN KEY (walletid) REFERENCES public.wallets(walletid);


--
-- TOC entry 5050 (class 2606 OID 16655)
-- Name: logs logs_userid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.logs
    ADD CONSTRAINT logs_userid_fkey FOREIGN KEY (userid) REFERENCES public.users(userid);


--
-- TOC entry 5051 (class 2606 OID 16674)
-- Name: userwallets userwallets_userid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.userwallets
    ADD CONSTRAINT userwallets_userid_fkey FOREIGN KEY (userid) REFERENCES public.users(userid);


--
-- TOC entry 5052 (class 2606 OID 16679)
-- Name: userwallets userwallets_walletid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.userwallets
    ADD CONSTRAINT userwallets_walletid_fkey FOREIGN KEY (walletid) REFERENCES public.wallets(walletid);


--
-- TOC entry 5053 (class 2606 OID 16684)
-- Name: userwallets userwallets_walletroleid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.userwallets
    ADD CONSTRAINT userwallets_walletroleid_fkey FOREIGN KEY (walletroleid) REFERENCES public.walletroles(walletroleid);


--
-- TOC entry 5054 (class 2606 OID 16713)
-- Name: walletgoals walletgoals_walletid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.walletgoals
    ADD CONSTRAINT walletgoals_walletid_fkey FOREIGN KEY (walletid) REFERENCES public.wallets(walletid);


-- Completed on 2026-03-27 18:25:02

--
-- PostgreSQL database dump complete
--

\unrestrict gdp14yHr3Q2lXsPChVfgmWp7I2ZnHqYOkQyeX5D29wQuTodNEUMlNeHjpDhbE13

--
-- Database "TestDb" dump
--

--
-- PostgreSQL database dump
--

\restrict M6K059eCoNI1gziU5hfw4ElyPkd8y6BAJT94bMJyhOeWFPNUCxeBWUGTmKOTt1E

-- Dumped from database version 18.3
-- Dumped by pg_dump version 18.3

-- Started on 2026-03-27 18:25:02

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 5011 (class 1262 OID 16388)
-- Name: TestDb; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE "TestDb" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_Australia.1256';


ALTER DATABASE "TestDb" OWNER TO postgres;

\unrestrict M6K059eCoNI1gziU5hfw4ElyPkd8y6BAJT94bMJyhOeWFPNUCxeBWUGTmKOTt1E
\connect "TestDb"
\restrict M6K059eCoNI1gziU5hfw4ElyPkd8y6BAJT94bMJyhOeWFPNUCxeBWUGTmKOTt1E

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 220 (class 1259 OID 16390)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    userid integer NOT NULL,
    username character varying(50)
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 16389)
-- Name: users_userid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_userid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.users_userid_seq OWNER TO postgres;

--
-- TOC entry 5012 (class 0 OID 0)
-- Dependencies: 219
-- Name: users_userid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_userid_seq OWNED BY public.users.userid;


--
-- TOC entry 4856 (class 2604 OID 16393)
-- Name: users userid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN userid SET DEFAULT nextval('public.users_userid_seq'::regclass);


--
-- TOC entry 4858 (class 2606 OID 16396)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (userid);


-- Completed on 2026-03-27 18:25:02

--
-- PostgreSQL database dump complete
--

\unrestrict M6K059eCoNI1gziU5hfw4ElyPkd8y6BAJT94bMJyhOeWFPNUCxeBWUGTmKOTt1E

--
-- Database "postgres" dump
--

\connect postgres

--
-- PostgreSQL database dump
--

\restrict yYUNyB8OgVngzNXuS4JOPKVj8o9GjwyVREcBu32SH3vR7yhItBQLuKK0uFyaLBe

-- Dumped from database version 18.3
-- Dumped by pg_dump version 18.3

-- Started on 2026-03-27 18:25:02

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

-- Completed on 2026-03-27 18:25:03

--
-- PostgreSQL database dump complete
--

\unrestrict yYUNyB8OgVngzNXuS4JOPKVj8o9GjwyVREcBu32SH3vR7yhItBQLuKK0uFyaLBe

-- Completed on 2026-03-27 18:25:03

--
-- PostgreSQL database cluster dump complete
--

