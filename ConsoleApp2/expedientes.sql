SELECT e.idpaciente, e.folio, dp.nombre, dp.apellidopaterno, dp.apellidomaterno from expediente e left outer join datospersonales dp on dp.id = e.idpaciente where e.idpaciente=44647;



select * from datospersonales where nombre like '%libertina%';
select * from paciente where id = 20486;
select * from datospersonales where id = 21699;

SELECT * from producto order by descripcion asc
/*

1. productos
2. usuarios
3. consultas




*/


/*productos consultas*/
select p.* from producto where p.idcategoria=5

/*productos cirugias*/
select p.* from producto where p.idcategoria=8 or p.idcategoria=2

/*productos estudiso*/
select p.* from producto where p.idcategoria=10 or p.idcategoria=11 or p.idcategoria=9 or p.idcategoria=12

/*productos otros*/
select p.* from producto where p.idcategoria=6 or p.idcategoria=0 or p.idcategoria=3 or p.idcategoria=7 select p.* from producto where p.idcategoria=6 or p.idcategoria=0 or p.idcategoria=3 or p.idcategoria=7


select * from expediente where idpaciente=2445;
select p.id as idpaciente, ex.folio, dp.nombre, dp.apellidopaterno, dp.apellidomaterno, dp.fechanacimiento, dp.sexo, dp.edocivil, dp.ocupacion,  ci.nombre as ciudad, mu.nombre as municipio, CONCAT(dox.calle, ' ', dox.numeroexterior) direccion, dox.colonia,  dox.telefono1, dox.telefono2, dox.correoelectronico1 from paciente   p left outer join expediente  ex on ex.idpaciente = p.id left outer join datospersonales dp on dp.id = p.iddatospersonales left outer join domicilio dox on dox.id = p.iddomicilio  left outer join ciudad ci on ci.id=dox.idciudad left outer join municipio mu on mu.id=ci.idmunicipio where   p.id=2445;


/*consultas*/
select e.* from evento e where e.idestatus=7 and e.idtipoitinerario=1


/*cirugias*/
select e.* from evento e where e.idestatus=20 and e.idtipoitinerario=2


/*Estudios*/
select e.* from evento e where e.idestatus=17 and e.idtipoitinerario=3