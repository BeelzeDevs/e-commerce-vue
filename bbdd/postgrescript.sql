/* Roles */
create table roles(
	id serial primary key,
	nombre varchar(50) not null
);

insert into roles(nombre)
values('Administrador'),('Cliente');

/* Usuarios */
create table usuarios(
	id serial primary key,
	rol_id int references roles(id),
	nombre varchar(50) not null,
	email varchar(100) unique not null,
	password_hash varchar(200) not null,
	fecha_registro timestamp default now(),
	estado bool default true
);

insert into usuarios(rol_id, nombre, email, password_hash)
values
(1,'Ignacio Aprile','ignacio@admin.com','admin123'),
(2,'Cliente 1', 'cliente1@cliente.com','cliente123');

/* Categorias */
create table categorias(
	id serial primary key,
	nombre varchar(100) not null
);

insert into categorias(nombre)
values ('Comestibles'),('Limpieza'),('Bebidas');

/* Productos */
create table productos(
	id serial primary key,
	categoria_id int references categorias(id),
	nombre varchar(100) not null,
	marca varchar(100) not null,
	descripcion varchar(255),
	precio decimal(10,2) not null,
	stock int default 0,
	imagen varchar(50),
	estado bool default true
);

insert into productos(categoria_id, nombre, marca, descripcion, precio,stock, imagen)
values
(1,'Manteca','Tasserenisima','Se podría leer una descripción detallada de un producto que todos deberían comprar, al menos una vez en la vida.', 29999.99,25,'img/manteca-taserenisima.webp'),
(2,'Detergente lavavajillas', 'Ala','Se podría leer una descripción detallada de un producto que todos deberían comprar, al menos una vez en la vida.', 39999.50,10,'img/ala-detergente.webp'),
(2,'Detergente ropa','Ala','Se podría leer una descripción detallada de un producto que todos deberían comprar, al menos una vez en la vida.', 35999.90,7,'img/ala-detergenteropa.webp'),
(2,'Lavandina','Ayudin','Se podría leer una descripción detallada de un producto que todos deberían comprar, al menos una vez en la vida.', 29999.99,25,'img/ayudin-lavandina.webp'),
(1,'Pan blanco', 'Bimbo','Se podría leer una descripción detallada de un producto que todos deberían comprar, al menos una vez en la vida.', 45999.50,10,'img/bimbo-panblanco.webp'),
(1,'Pan integral','Bimbo','Se podría leer una descripción detallada de un producto que todos deberían comprar, al menos una vez en la vida.', 35999.90,7,'img/bimbo-panintegral.webp'),
(3,'Whisky','Blue label','Se podría leer una descripción detallada de un producto que todos deberían comprar, al menos una vez en la vida.', 29999.99,25,'img/bluelabel.webp'),
(2,'Detergente', 'Cif','Se podría leer una descripción detallada de un producto que todos deberían comprar, al menos una vez en la vida.', 30999.50,10,'img/cif-detergente.webp'),
(3,'Gaseosa 1.5l','Coca Cola','Se podría leer una descripción detallada de un producto que todos deberían comprar, al menos una vez en la vida.', 35999.90,7,'img/cocacola-1-5lit.webp'),
(3,'Gaseosa 2.25l','Coca Cola','Se podría leer una descripción detallada de un producto que todos deberían comprar, al menos una vez en la vida.', 35999.90,7,'img/cocacola-2-25l.webp'),
(1,'Fideos','Don vicente','Se podría leer una descripción detallada de un producto que todos deberían comprar, al menos una vez en la vida.', 35999.90,7,'img/donvicente-fideos.webp'),
(1,'Palitos surimi','Kanikama','Se podría leer una descripción detallada de un producto que todos deberían comprar, al menos una vez en la vida.', 35999.90,7,'img/kanikama-palitossurimi.webp'),
(1,'Lomitos de atún','LaAnonima','Se podría leer una descripción detallada de un producto que todos deberían comprar, al menos una vez en la vida.', 35999.90,7,'img/laanonima-lomitosdeatun.webp'),
(1,'Hamburguesas','Union ganadera','Se podría leer una descripción detallada de un producto que todos deberían comprar, al menos una vez en la vida.', 35999.90,7,'img/unionganadera-hamburguesas.webp'),
(3,'Whisky','Jack Daniels','Se podría leer una descripción detallada de un producto que todos deberían comprar, al menos una vez en la vida.', 35999.90,7,'img/whisky-jack.webp');

/* Ordenes */
create table ordenes(
 id serial primary key,
 usuario_id int references usuarios(id),
 fecha timestamp default now(),
 total decimal(10,2) not null
);

create table detalle_ordenes(
 orden_id int references ordenes(id) on delete cascade,
 producto_id int references productos(id),
 cantidad int not null,
 subtotal decimal(10,2) not null,

 primary key (orden_id,producto_id)
);


Insert into ordenes(usuario_id,total)
values(2,29999.99);

insert into detalle_ordenes(orden_id,producto_id,cantidad,subtotal)
values
(1,1,1,29999.99);


