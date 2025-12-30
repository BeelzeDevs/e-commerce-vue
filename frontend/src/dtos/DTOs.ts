export type ResultadoPaginado<T> = {
    items : T[],
    page : number,
    pageSize : number,
    totalItems : number,
    totalPages : number,
};

/* Pagination query y ProductoQuery están como guía para saber que parametros recibe el backend sin mirar sus DTO para filtros */
export class PaginationQuery {
    pageSize? : number ;
    page? : number;

};
export class ProductoQuery extends PaginationQuery{
    categoriaId? : number;
    search? : string;
    estado? : boolean;
}

export type ApiResponse<T> = {
    results : T | T[] | ResultSuccess | ResultError

};
export type ResultSuccess = {
    successMessage : string;
}
export type ResultError = {
    errorMessage : string;
};

export type CategoriaReadDTO = {
    id : number,
    nombre : string
};

export const esResultError = (results : any): results is ResultError => {
    return (results && typeof results.errorMessage === 'string');
};

export const esResultSuccess = (results : any) : results is ResultSuccess => {
    return (results && typeof results.successMessage === 'string');
};

export type ProductoReadDTO = {
    id : number,
    categoria : CategoriaReadDTO,
    nombre : string,
    marca : string,
    descripcion : string,
    precio : number,
    stock : number,
    imagen : string,
    estado : boolean
};
export type LoginDTO = {
    nombre : string,
    email : string,
    token: string,
    rol : string,
    expiration: Date,
    errorMessage? : string
};

export type RolReadDTO = {
    id : number,
    nombre: string,
};

export type UsuarioReadDTO = {
    id : number,
    rol : RolReadDTO,
    nombre : string,
    email : string,
    fechaRegistro: Date,
    estado : boolean
};

export type estadoOrden = 'Pendiente' | 'Pagado' | 'Enviado' | 'Cancelado';

export type OrdenReadDTO = {
    id : number,
    usuario: UsuarioReadDTO,
    fecha : Date,
    total : number,
    estado : estadoOrden
};

export type DetalleReadDTO = {
    ordenId : number,
    producto : ProductoReadDTO,
    precio_Producto : number,
    cantidad : number,
    subtotal : number
};
    // Stats
export type VentasTotalesDTO = {
    cantidadVentas : number,
    ventasTotales : number,
    ticketPromedio : number
};
export type VentasPorMesDTO = {
    año : number,
    mes : number,
    totalVentas : number,
    cantidadOrdenes : number
};
export type TopProductoDTO = {
    productoId : number,
    nombre : string,
    cantidadVendidad : number,
    totalFacturado : number,
};
export type OrdenesPorEstadoDTO = {
    estado : estadoOrden,
    cantidad : number
};


// Updates
export type ProductoUpdateDTO = {
    nombre : string,
    categoriaId : number,
    marca : string,
    descripcion : string,
    precio : number,
    stock : number,
    imagen : string,
    estado : boolean
};

// Creates
export type ProductoCreateDTO = {
    nombre : string,
    categoriaId : number,
    marca : string,
    descripcion : string,
    precio : number,
    stock : number,
    imagen : string,
    estado : boolean
}

export type carritoItemBackend = {
    productoId : number,
    cantidad : number,
}

export type OrdenCreateDTO = {
    carritoItems : carritoItemBackend[],
    fecha? : Date    
};

export type CreateUsuarioDTO = {
    nombre : string,
    email : string,
    password : string,
};

export type CreateAdminDTO = {
    nombre : string,
    email : string,
    password : string,
}