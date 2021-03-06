PGDMP     2    "            	    y            postgres    13.2    13.2     ?           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            ?           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            ?           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            ?           1262    13442    postgres    DATABASE     e   CREATE DATABASE postgres WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'Russian_Russia.1251';
    DROP DATABASE postgres;
                postgres    false            ?           0    0    DATABASE postgres    COMMENT     N   COMMENT ON DATABASE postgres IS 'default administrative connection database';
                   postgres    false    2996                        3079    16384 	   adminpack 	   EXTENSION     A   CREATE EXTENSION IF NOT EXISTS adminpack WITH SCHEMA pg_catalog;
    DROP EXTENSION adminpack;
                   false            ?           0    0    EXTENSION adminpack    COMMENT     M   COMMENT ON EXTENSION adminpack IS 'administrative functions for PostgreSQL';
                        false    2            ?            1255    16501    delete_usr(integer)    FUNCTION     ?   CREATE FUNCTION public.delete_usr(_id integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$

begin
 delete from Users
 where id = _id;
 if found then --deleted successfully
  return 1;
 else
  return 0;
 end if;
end

$$;
 .   DROP FUNCTION public.delete_usr(_id integer);
       public          postgres    false            ?            1255    16498 5   insert(character varying, character varying, integer)    FUNCTION     P  CREATE FUNCTION public.insert(firstname character varying, lastname character varying, age integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$

begin
 insert into Users(FirstName, LastName, Age)
 values(firstname, lastname, age);
 if found then --inserted successfully
  return 1;
 else return 0; -- inserted fail
 end if;
end

$$;
 c   DROP FUNCTION public.insert(firstname character varying, lastname character varying, age integer);
       public          postgres    false            ?            1255    16506    make_tsvector(text, text)    FUNCTION       CREATE FUNCTION public.make_tsvector(firstname text, lastname text) RETURNS tsvector
    LANGUAGE plpgsql IMMUTABLE
    AS $$
BEGIN
  RETURN (setweight(to_tsvector('english', firstname),'A') ||
    setweight(to_tsvector('english', lastname), 'B'));
END
$$;
 C   DROP FUNCTION public.make_tsvector(firstname text, lastname text);
       public          postgres    false            ?            1255    16510     select_search(character varying)    FUNCTION     I  CREATE FUNCTION public.select_search(text_search character varying) RETURNS TABLE(id integer, firstname character varying, lastname character varying, age integer)
    LANGUAGE plpgsql
    AS $$
begin
  return query
 SELECT * FROM users WHERE
  make_tsvector(users.firstname, users.lastname) @@ to_tsquery(text_search);
 end
$$;
 C   DROP FUNCTION public.select_search(text_search character varying);
       public          postgres    false            ?            1255    16499    select_users()    FUNCTION     ?   CREATE FUNCTION public.select_users() RETURNS TABLE(id integer, firstname character varying, lastname character varying, age integer)
    LANGUAGE plpgsql
    AS $$

 begin
  return query
 select * from Users order by id;
 end
 
$$;
 %   DROP FUNCTION public.select_users();
       public          postgres    false            ?            1255    16503 D   update_users(integer, character varying, character varying, integer)    FUNCTION     {  CREATE FUNCTION public.update_users(_id integer, _firstname character varying, _lastname character varying, _age integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$
begin
  update Users
 set 
  FirstName = _firstname,
  LastName = _lastname ,
  Age = _age 
 where _id = id;
 if found then --updated successfully
  return 1;
 else --updated fail
  return 0;
 end if;
 end
$$;
 y   DROP FUNCTION public.update_users(_id integer, _firstname character varying, _lastname character varying, _age integer);
       public          postgres    false            ?            1259    16494    users    TABLE     ?   CREATE TABLE public.users (
    id integer NOT NULL,
    firstname character varying(20) NOT NULL,
    lastname character varying(20) NOT NULL,
    age integer NOT NULL
);
    DROP TABLE public.users;
       public         heap    postgres    false            ?            1259    16492    users_id_seq    SEQUENCE     ?   CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 #   DROP SEQUENCE public.users_id_seq;
       public          postgres    false    202            ?           0    0    users_id_seq    SEQUENCE OWNED BY     =   ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;
          public          postgres    false    201            )           2604    16497    users id    DEFAULT     d   ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);
 7   ALTER TABLE public.users ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    201    202    202            ?          0    16494    users 
   TABLE DATA           =   COPY public.users (id, firstname, lastname, age) FROM stdin;
    public          postgres    false    202   M       ?           0    0    users_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('public.users_id_seq', 12, true);
          public          postgres    false    201            *           1259    16507    idx_fts_articles    INDEX     u   CREATE INDEX idx_fts_articles ON public.users USING gin (public.make_tsvector((firstname)::text, (lastname)::text));
 $   DROP INDEX public.idx_fts_articles;
       public            postgres    false    202    202    203    202            ?   E   x?3?*-.I???J?L??/?42?????K,?.?I??M̃?p?&??3?2ӲKs ?1z\\\ 9??     