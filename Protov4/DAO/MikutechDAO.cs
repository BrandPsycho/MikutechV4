﻿using Protov4.DTO;

namespace Protov4.DAO
{
    public class MikutechDAO : MikuTechFactory
    {   ProductoDAO productoDAO;
        CarritoDAO carritoDAO;
        AuditoriaDAO auditoriaDAO;
        DashboardDAO dashDAO;
        // Indica si se ha realizado un pedido
        public static bool pedido = false;
        public MikutechDAO(IConfiguration configuration)
        {
            productoDAO = new ProductoDAO(configuration);
            carritoDAO = new CarritoDAO(configuration);
            auditoriaDAO = new AuditoriaDAO(configuration);
            dashDAO = new DashboardDAO(configuration);
        }
        // Actualiza los detalles de un pedido
        public override void ActualizarPedido(int id_pedido,decimal pago_total, int tipoestado , string Ciudad_envio, string Calle_principal, string Calle_secundaria, int id_tipo_pago, DateTime fecha_pedido)
        {
            carritoDAO.ActualizarPedido(id_pedido, pago_total,tipoestado, Ciudad_envio, Calle_principal, Calle_secundaria, id_tipo_pago, fecha_pedido);
        }
        // Actualiza los detalles de un pedido

        public override void ActualizarPedidoDetalle(string id_producto, int id_pedido, int cantidad, decimal subtotal_producto)
        {
            carritoDAO.ActualizarPedidoDetalle(id_producto,id_pedido, cantidad, subtotal_producto);
        }
        // Elimina un producto del carrito de compras
        public override void EliminarProductoCarrito(int id, string idproducto)
        {
            carritoDAO.EliminarProductoCarrito(id, idproducto);
        }
        // Obtiene una lista de todos los productos
        public override List<ProductoDTO> GetAllProductos(string tipo, string busqueda)
        {
            return productoDAO.ObtenerProductos(tipo,busqueda);
        }
        // Obtiene detalles de un producto específico
        public override List<ProductoDTO> GetSeleccion(string id)
        {
            return productoDAO.ObtenerSeleccion(id);
        }
        // Inserta un nuevo elemento en el carrito de compras
        public override void insertarCarrito(int id_pedido, string id_producto, decimal precio, int cantidad, decimal subtotal)
        {
            carritoDAO.InsertarPedidoDetalle(id_pedido, id_producto, precio, cantidad, subtotal);
        }
        // Inserta un nuevo detalle de pedido
        public override void insertarPedido_Detalle(int id_pedido, string id_producto, decimal precio, int cantidad, decimal subtotal)
        {
            carritoDAO.InsertarPedidoDetalle(id_pedido, id_producto, precio, cantidad, subtotal);
        }
        // Obtiene una lista de elementos en el carrito de compras
        public override List<PedidoDetalleDTO> ObtenerCarrito()
        {
            return carritoDAO.ObtenerCarrito();
        }
        // Obtiene una lista de elementos del carrito con detalles completos
        public override List<CarritoFullDTO> ObtenerCarritoFull(int id)
        {
            return carritoDAO.ObtenerCarritoFull(id);
        }
        // Obtiene el último ID de pedido registrado
        public override int ObtenerIdPedido()
        {
            return carritoDAO.ObtenerIdPedido();
        }
        // Registra un nuevo pedido
        public override void RegistrarPedido(int id_cliente)
        {
             carritoDAO.RegistrarPedido(id_cliente);
        }
        // Obtiene detalles de un pedido específico
        public override List<PedidoDTO> ObtenerPedidoPorId(int idPedido)
        {
            return carritoDAO.ObtenerPedidoPorId(idPedido);
        }
        //Método que actualiza las existencias de un producto
        public override void ActualizarExistencias(string id, int cantidad)
        {
            productoDAO.ActualizarExistencias(id,cantidad);
        }
        //Métodoq ue regresa una lista con la auditoria
        public override List<AuditoriaDTO> ObtenerAuditoria()
        {
            return auditoriaDAO.ListarAuditoria();
        }
        //Método para insertar nuevos productos
        public override void InsertarProducto(string nombre,string imagen, float precio, string Marca, int existencia, string tipo, string fabricante, string modelo, string velocidad, string Zócalo, string TamañoVRAM, string Interfaz, string TecnologiaRAM, string tamañomemoria, string Almacenamiento, List<string> Descripcion)
        {
            productoDAO.InsertarProducto(nombre,imagen,precio,Marca,existencia,tipo,fabricante,modelo,velocidad,Zócalo,TamañoVRAM,Interfaz,TecnologiaRAM,tamañomemoria,Almacenamiento,Descripcion);
        }
        //Llama al metodo para eliminar los productos
        public override void eliminarProducto(string id)
        {
            productoDAO.eliminarProducto(id);
        }
        //Metodo para llamar a la actulizacion de los productos
        public override void ActualizarProducto(string _id, string nombre, double precio, string tipo, string imagenBase64, string Marca, int existencia, string Fabricante, string Modelo, string Velocidad, string Zocalo, string TamañoVram, string Interfaz, string Tamañomemoria, string TecnologiaRam, string Almacenamiento, List<string> Descripcion)
        {
            productoDAO.ActualizarProducto(_id,nombre,precio,tipo,imagenBase64,Marca,existencia,Fabricante,Modelo,Velocidad,Zocalo,TamañoVram,Interfaz,Tamañomemoria,TecnologiaRam,Almacenamiento,Descripcion);
        }
        //Regresa la lista de los productos mas vendidos
        public override List<DashboardDTO> MasVendidos()
        {
            return dashDAO.MasVendidos();
        }
    }
}
    