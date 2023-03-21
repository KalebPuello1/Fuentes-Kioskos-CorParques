using CorParques.Datos.Contratos;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioControlParqueadero : IServicioControlParqueadero
    {
        IRepositorioControlParqueadero _repositorio;
        IRepositorioTarifasParqueadero _repositorioTarifa;
        IRepositorioTipoVehiculoPorParqueadero _repositorioTipoVehiculoPorParqueadero;
        IRepositorioParametros _repositorioParametros;
        IRepositorioConvenioParqueadero _repositorioConvenioParqueadero;
        IRepositorioFactura _repositorioFactura;

        public ServicioControlParqueadero(IRepositorioControlParqueadero repositorio, IRepositorioTarifasParqueadero repositorioTarifa, IRepositorioTipoVehiculoPorParqueadero repositorioTipoVehiculoPorParqueadero, IRepositorioParametros repositorioParametros, IRepositorioConvenioParqueadero repositorioConvenioParqueadero, IRepositorioFactura repositorioFactura)
        {
            _repositorio = repositorio;
            _repositorioTarifa = repositorioTarifa;
            _repositorioTipoVehiculoPorParqueadero = repositorioTipoVehiculoPorParqueadero;
            _repositorioParametros = repositorioParametros;
            _repositorioConvenioParqueadero = repositorioConvenioParqueadero;
            _repositorioFactura = repositorioFactura;
        }

        public ControlParqueadero Insertar(ControlParqueadero ingresoPaqueadero, out string Mensaje)
        {
            Mensaje = "";
            //se valida que vengan los datos
            if (!string.IsNullOrEmpty(ingresoPaqueadero.Placa)
                && ingresoPaqueadero.IdTipoVehiculo != 0
                && ingresoPaqueadero.CodUsuarioIngreso != 0)
            {

                IEnumerable<TipoVehiculoPorParqueadero> listaDisponibilidad = ObtenerDisponibilidad();
                bool hayDisponible = false;
                foreach (var item in listaDisponibilidad)
                {
                    if (item.IdTipoVehiculo == ingresoPaqueadero.IdTipoVehiculo)
                    {
                        if (item.Cantidad >= 1)
                        {
                            hayDisponible = true;
                        }
                    }
                }
                if (!hayDisponible)
                {
                    Mensaje = "No hay disponibilidad para el tipo de vehículo";
                }
                else
                {
                    var _listaValidacion = _repositorio.ObtenerLista($"WHERE Placa = '{ingresoPaqueadero.Placa}' and FechaHoraSalida IS NULL and IdUsuarioSalida IS NULL and IdEstado = 1");
                    if (_listaValidacion.Count() == 0)
                    {
                        //Se regitra la hora
                        ingresoPaqueadero.FechaHoraIngreso = Utilidades.FechaActualColombia;
                        //estado 1, ingresado
                        ingresoPaqueadero.IdEstado = (int)Enumerador.Estados.Activo;
                        //inserta
                        var resp = _repositorio.Insertar(ref ingresoPaqueadero);
                        if (resp > 0)
                        {
                            Mensaje = "";
                            ingresoPaqueadero.Id = resp;
                            return ingresoPaqueadero;
                        }
                        else
                        {
                            Mensaje = "Error";
                        }
                    }
                    else
                    {
                        Mensaje = "No pudo ser ingresado, el vehículo ya se encuentra en el parqueadero";
                    }
                }
            }
            else
            {
                Mensaje = "Datos Inválidos";
            }
            return null;

        }

        public ControlParqueadero RegistrarSalida(ControlParqueadero ingreso, out string Mensaje)
        {
            Mensaje = "";
            ControlParqueadero _salida = _repositorio.Obtener(ingreso.Id);

            if (_salida == null)
            {
                _salida = null;
                Mensaje = "Registro Inválido";
            }
            else if (_salida.IdEstado == (int)Enumerador.Estados.Activo)
            {
                var tieneAut = _repositorioConvenioParqueadero.ObtenerPorPlaca(_salida.Placa);

                if (tieneAut != null)
                {
                    _salida.CodUsuarioSalida = ingreso.CodUsuarioSalida;
                    _salida.FechaHoraSalida = Utilidades.FechaActualColombia;
                    _salida.IdEstado = 3;

                    bool resp = _repositorio.Actualizar(ref _salida);
                    if (resp)
                    {
                        Mensaje = "Salida por autorización";
                    }
                    else
                    {
                        _salida = null;
                        Mensaje = "Ha ocurrido un error inesperado";
                    }
                }
                else
                {
                    Parametro paramTiempoGracia = _repositorioParametros.ObtenerParametroPorNombre("TiempoGraciaIngresoParqueadero");
                    int valorTiempoGracia = 0;
                    int.TryParse(paramTiempoGracia.Valor, out valorTiempoGracia);

                    DateTime FechaHoraSalida = Utilidades.FechaActualColombia;
                    var tiempoCalculado = FechaHoraSalida - _salida.FechaHoraIngreso;

                    if (tiempoCalculado.TotalMinutes <= valorTiempoGracia)
                    {
                        _salida.CodUsuarioSalida = ingreso.CodUsuarioSalida;
                        _salida.FechaHoraSalida = FechaHoraSalida;
                        _salida.IdEstado = 3;

                        bool resp = _repositorio.Actualizar(ref _salida);
                        if (resp)
                        {
                            Mensaje = "Salida por tiempo de gracia.";
                        }
                        else
                        {
                            _salida = null;
                            Mensaje = "Ha ocurrido un error inesperado";
                        }

                    }
                    else
                    {
                        _salida = null;
                        Mensaje = "El vehículo no ha realizado el pago";
                    }
                }
            }
            else if (_salida.IdEstado == (int)Enumerador.Estados.Anulado)
            {
                _salida = null;
                Mensaje = "El vehículo ya salió del parqueadero";
            }
            else if (_salida.IdEstado == (int)Enumerador.Estados.Inactivo)
            {
                DateTime FechaSalida = Utilidades.FechaActualColombia;
                bool haSuperadoTiempoGracia = false;
                Parametro paramTiempoGracia = _repositorioParametros.ObtenerParametroPorNombre("TiempoGraciaParaSalirParqueadero");
                int valorTiempoGracia = 0;
                int.TryParse(paramTiempoGracia.Valor, out valorTiempoGracia);

                TarifasParqueadero objTarifaAplicada = _repositorioTarifa.Obtener(_salida.IdTarifaParqueadero);

                if (objTarifaAplicada != null)
                {
                    if (objTarifaAplicada.IdTipoTarifaParqueadero == 1)
                    {
                        DateTime? fechaPago = _repositorioFactura.ObtenerFechaPago(_salida.Id);
                        if (fechaPago != null)
                        {
                            var tiempoCalculado = FechaSalida - fechaPago;
                            if (tiempoCalculado.Value.TotalMinutes > valorTiempoGracia)
                            {
                                haSuperadoTiempoGracia = true;
                            }
                        }
                    }
                }
                
                if (haSuperadoTiempoGracia)
                {
                    _salida = null;
                    Mensaje = "Ha excedido el tiempo después del pago";
                }
                else
                {
                    _salida.CodUsuarioSalida = ingreso.CodUsuarioSalida;
                    _salida.FechaHoraSalida = FechaSalida;
                    _salida.IdEstado = 3;

                    bool resp = _repositorio.Actualizar(ref _salida);
                    if (resp)
                    {
                        Mensaje = "Salida registrada con éxito";
                    }
                    else
                    {
                        _salida = null;
                        Mensaje = "Ha ocurrido un error inesperado";
                    }
                }
            }

            return _salida;
        }

        public ControlParqueadero CalcularPago(ControlParqueadero ingreso, out string Mensaje)
        {
            Mensaje = "";
            ingreso = _repositorio.Obtener(ingreso.Id);

            if (ingreso == null)
            {
                ingreso = null;
                Mensaje = "Registro Inválido";
            }
            else if (ingreso.IdEstado == (int)Enumerador.Estados.Inactivo)
            {
                ingreso = null;
                Mensaje = "El vehículo ya registra pago";
            }
            else if (ingreso.IdEstado == (int)Enumerador.Estados.Anulado)
            {
                ingreso = null;
                Mensaje = "El vehículo ya salió del parquedero";
            }
            else if (!string.IsNullOrEmpty(ingreso.Placa) && ingreso.Id != 0) //se valida que vengan los datos
            {
                //Se regitra la hora
                DateTime FechaHoraPago = Utilidades.FechaActualColombia;
                var listaTarifas = _repositorioTarifa.ObtenerLista();

                var listaTarifasTipoVehiculo = listaTarifas.Where(l => l.IdTipoVehiculo == ingreso.IdTipoVehiculo);

                if (listaTarifasTipoVehiculo == null)
                {
                    ingreso = null;
                    Mensaje = "No hay tarifas configuradas";
                }
                else
                { 
                    var tieneAut = _repositorioConvenioParqueadero.ObtenerPorPlaca(ingreso.Placa);
                    if (tieneAut != null)
                    {
                        //TarifasParqueadero objTarifaMinuto = listaTarifasTipoVehiculo.FirstOrDefault(t => t.IdTipoTarifaParqueadero == 3);
                        ingreso.ValorPago = 0;
                        ingreso.IdTarifaParqueadero = 0;
                    }
                    else
                    {
                        var tiempoCalculado = FechaHoraPago - ingreso.FechaHoraIngreso;
                        //Validar si esta en el tiempo de gracia 
                        Parametro paramTiempoGracia = _repositorioParametros.ObtenerParametroPorNombre("TiempoGraciaParaSalirParqueadero");
                        if(tiempoCalculado.TotalMinutes <= int.Parse(paramTiempoGracia.Valor.Trim()))
                        {
                            ingreso = null;
                            Mensaje = "Boleta de parqueadero en el tiempo de gracia";
                            return ingreso;
                        }

                        //*Calculo por minutos**//
                        TarifasParqueadero objTarifaMinuto = listaTarifasTipoVehiculo.FirstOrDefault(t => t.IdTipoTarifaParqueadero == 1);
                        if (objTarifaMinuto == null)
                        {
                            ingreso = null;
                            Mensaje = "No hay tarifa por minuto configurada";
                        }
                        else
                        {
                            double valorPorMinutos = 0;

                            if (Math.Truncate(tiempoCalculado.TotalMinutes) == 0)
                                valorPorMinutos = (double)objTarifaMinuto.Valor * 1;
                            else
                                valorPorMinutos = (double)objTarifaMinuto.Valor * (double)Math.Truncate(tiempoCalculado.TotalMinutes);

                            //*Calculo Plena cada 24 horas**//
                            TarifasParqueadero objTarifaPlena = listaTarifasTipoVehiculo.FirstOrDefault(t => t.IdTipoTarifaParqueadero == 2);
                            if (objTarifaPlena == null)
                            {
                                ingreso = null;
                                Mensaje = "No hay tarifa plena configurada";
                            }
                            else
                            {

                                if ((objTarifaMinuto.IdEstado == (int)Enumerador.Estados.Inactivo)
                                    && (objTarifaPlena.IdEstado == (int)Enumerador.Estados.Inactivo))
                                {
                                    ingreso = null;
                                    Mensaje = "No hay tarifa plena configurada";
                                }
                                else
                                {

                                    double valorPorPlena = 0;

                                    if (Math.Truncate(tiempoCalculado.TotalDays) == 0)
                                        valorPorPlena = (double)objTarifaPlena.Valor * 1;
                                    else
                                        valorPorPlena = (double)objTarifaPlena.Valor * (double)Math.Truncate(tiempoCalculado.TotalDays);

                                    //Si el valor por minutos supera la plena se cobra plena, o si la tarifa pr o minutos el por minutos esta 
                                    //deshabilitado toma la plena -- 25/10/2017
                                    if ((valorPorMinutos > valorPorPlena && objTarifaPlena.IdEstado == (int)Enumerador.Estados.Activo) || objTarifaMinuto.IdEstado == (int)Enumerador.Estados.Inactivo)
                                    {
                                        ingreso.ValorPago = valorPorPlena;
                                        ingreso.IdTarifaParqueadero = objTarifaPlena.Id;
                                    }
                                    //Si no aplica por minutos
                                    else
                                    {
                                        ingreso.ValorPago = valorPorMinutos;
                                        ingreso.IdTarifaParqueadero = objTarifaMinuto.Id;
                                    }

                                    bool resp = _repositorio.Actualizar(ref ingreso);
                                }//--> Else if (objTarifaPlena == null)
                            }
                        }//-->Else if (objTarifaMinuto == null)
                    }
                }
            }

            return ingreso;

        }

        public IEnumerable<TipoVehiculoPorParqueadero> ObtenerDisponibilidad()
        {
            //Obtiene La cantidad de espacios por parqueadero
            Parametro paramCantidadEspacios = _repositorioParametros.ObtenerParametroPorNombre("CantidadTotalEspaciosParqueadero");
            int iCantidadTotalEspaciosParqueadero = 0;
            int.TryParse(paramCantidadEspacios.Valor, out iCantidadTotalEspaciosParqueadero);

            //Obtiene lista disponibilidad
            IEnumerable<TipoVehiculoPorParqueadero> listaDispo = _repositorioTipoVehiculoPorParqueadero.ObtenerLista().Where(l => l.IdEstado == 1).OrderBy(r => r.EspaciosReservados).ThenBy(r => r.IdTipoVehiculo);
            int totalEspaciosReservados = listaDispo.Sum(x => x.EspaciosReservados);

            //A la cantidad total de paqueaderos, resta los espacios reservados
            iCantidadTotalEspaciosParqueadero = iCantidadTotalEspaciosParqueadero - totalEspaciosReservados;

            //Consulta los vehiculos que esten dentro del parqueadero
            IEnumerable<ControlParqueadero> listaIngresados = _repositorio.ObtenerLista($"WHERE IdEstado IN ( {(int)Enumerador.Estados.Activo}, {(int)Enumerador.Estados.Inactivo})");

            //Actualiza iCantidadTotalEspaciosParqueadero, restando lo que hay actualmente en parqueadero
            foreach (TipoVehiculoPorParqueadero item in listaDispo.Where(r => r.EspaciosReservados == 0))
            {
                int cantidadTipoVeh = listaIngresados.Where(p => p.IdTipoVehiculo == item.IdTipoVehiculo).Count();
                iCantidadTotalEspaciosParqueadero = iCantidadTotalEspaciosParqueadero - (int)(cantidadTipoVeh * item.EspaciosPorVehiculo);
            }

            //Actualiza la cantidad disponible por cada tipo de vehiculo
            foreach (TipoVehiculoPorParqueadero item in listaDispo)
            {
                int cantidadActual = listaIngresados.Where(p => p.IdTipoVehiculo == item.IdTipoVehiculo).Count();
                if (item.EspaciosReservados == 0)
                {
                    item.Cantidad = (int)(iCantidadTotalEspaciosParqueadero / item.EspaciosPorVehiculo);
                }
                else
                {
                    item.Cantidad = (int)(item.EspaciosReservados * (1 / item.EspaciosPorVehiculo)) - cantidadActual;
                }
            }

            return listaDispo;
        }


        public ControlParqueadero ObtenerPorPlaca(string Placa)
        {
            ControlParqueadero _ingreso = null;
            var listaconsultada = _repositorio.ObtenerLista($"where PLACA = '{Placa}' AND IdEstado IN (1,2)");

            if (listaconsultada != null && listaconsultada.Count() > 0)
            {
                _ingreso = listaconsultada.FirstOrDefault();
            }
            
            return _ingreso;
        }

        public ControlParqueadero Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        //public ControlParqueadero ObtenerPorPlaca(string Placa)
        //{
        //    ControlParqueadero _ingreso = _repositorio.ObtenerLista($"where PLACA = '{Placa}' and FECHAHORA_SALIDA IS NULL and CODUSUARIO_SALIDA IS NULL and ID_TARIFA IS NULL and VALOR_PAGO IS NULL").FirstOrDefault();
        //    return _ingreso;
        //}

        //public bool Actualizar(ControlParqueadero salida)
        //{
        //    return _repositorio.Actualizar(ref salida);
        //}
    }
}
