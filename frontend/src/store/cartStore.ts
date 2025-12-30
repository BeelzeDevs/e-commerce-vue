import type { ProductoReadDTO } from "@/dtos/DTOs";
import { defineStore } from "pinia";

export type CarritoItem = {
    producto : ProductoReadDTO,
    cantidad : number,
};

export const useCartStore = defineStore("cart",{
    state : ()=>({
        items : [] as CarritoItem[]
    }),

    getters : {
        cantidadItems : state => state.items.reduce((acum,carritoItem)=> acum + carritoItem.cantidad,0),

        totalPrecio : state => state.items.reduce((acum,item)=> acum + item.producto.precio * item.cantidad ,0)
    },

    actions : {
        agregarProducto(prod : ProductoReadDTO){
            const item = this.items.find(i => i.producto.id == prod.id);
            const stockDisponible = this.getStockDisponible(prod);

            if(stockDisponible <= 0) return;
            if(item) item.cantidad++;
            else{
                this.items.push({
                    producto : prod,
                    cantidad : 1,
                });
            }

            this.guardarCarrito();
        },
        eliminarProducto(prod : ProductoReadDTO){
            this.items = this.items.filter(i=> i.producto.id != prod.id );
            this.guardarCarrito();
        },
        vaciarCarrito(){
            this.items = [];
            this.guardarCarrito();
        },
        
        bajarCantidadAComprar(prod : ProductoReadDTO){
            const item = this.items.find(i=>i.producto.id == prod.id);
            
            if(!item) return;
            if(item.cantidad > 1) item.cantidad--;
            else this.eliminarProducto(prod);
            this.guardarCarrito();
        },
        aumentarCantidadAComprar(prod : ProductoReadDTO){
            const item = this.items.find(i=>i.producto.id == prod.id);
            if(!item) return;

            if(this.getStockDisponible(prod) > 0) item.cantidad++;

            this.guardarCarrito();
        },
        existeEnCarrito(prod : ProductoReadDTO) : boolean{
            const existe = this.items.find(i=> i.producto.id == prod.id);
            return existe ? true : false;
        },
        buscarCarritoItem(prod : ProductoReadDTO){
            return this.items.find(i => i.producto.id === prod.id);
        },
        guardarCarrito(){
            localStorage.setItem('carrito',JSON.stringify(this.items));
        },
        getStockDisponible(prod : ProductoReadDTO): number {
            const item = this.items.find(i=> i.producto.id == prod.id);
            const stock = prod.stock - (item?.cantidad ?? 0);
            return stock;
        },
        getCantidadEnCarrito(prod : ProductoReadDTO): number{
            const item = this.items.find(i=> i.producto.id == prod.id);
            const cantidadYaPedida = item ? item.cantidad : 0;
            return cantidadYaPedida;
        },
        cargarCarrito(){
            const data = JSON.parse(localStorage.getItem("carrito") || "null" ) as CarritoItem[] | null;
            if(data) this.items = data;
        }
    }
});