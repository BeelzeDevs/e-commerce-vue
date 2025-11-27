export type ApiResponse<T> = {
    results? : T[],
    successMessage?: string,
    errorMessage?: string,
};

export type CategoriaReadDTO = {
    id : number,
    nombre : string
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
    email : string,
    token: string,
    rol : string,
    expiration: Date
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
