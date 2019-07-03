using OpenInvoicePeru.Comun.Dto.Intercambio;
using OpenInvoicePeru.Comun.Dto.Modelos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace OpenInvoicePeru.ApiClientCSharp
{
    class Program
    {
        private const string UrlSunat = "https://e-beta.sunat.gob.pe/ol-ti-itcpfegem-beta/billService";
        private const string UrlOtroCpe = "https://e-beta.sunat.gob.pe/ol-ti-itemision-otroscpe-gem-beta/billService";
        private const string UrlGuiaRemision = "https://e-beta.sunat.gob.pe/ol-ti-itemision-guia-gem-beta/billService";
        private const string FormatoFecha = "yyyy-MM-dd";

        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "OpenInvoicePeru - Prueba de Envío de Documentos Electrónicos con UBL 2.1";

            CrearFactura();
            CrearFacturaExportacion();
            CrearFacturaConDescuento();
            CrearFacturaExonerada();
            CrearFacturaInafecta();
            CrearFacturaConPlaca();
            CrearFacturaConDetraccion();
            CrearFacturaConDetraccionTransportes();
            CrearFacturaConAnticipo();
            CrearFacturaGratuita();
            CrearBoleta();
            CrearNotaCredito();
            CrearNotaDebito();
            CrearResumenDiario();
            CrearComunicacionBaja();
            CrearDocumentoRetencion();
            CrearDocumentoPercepcion();
            CrearGuiaRemision();
            DescargarComprobante();
        }

        private static Compania CrearEmisor()
        {
            return new Compania
            {
                NroDocumento = "20257471609",
                TipoDocumento = "6",
                NombreComercial = "FRAMEWORK PERU",
                NombreLegal = "EMPRESA DE SOFTWARE S.A.C.",
                CodigoAnexo = "0001"
            };
        }

        private static Negocio ToNegocio(Compania compania)
        {
            return new Negocio
            {
                NroDocumento = compania.NroDocumento,
                TipoDocumento = compania.TipoDocumento,
                NombreComercial = compania.NombreComercial,
                NombreLegal = compania.NombreLegal,
                Distrito = "LIMA",
                Provincia = "LIMA",
                Departamento = "LIMA",
                Direccion = "AV. LIMA 123",
                Ubigeo = "150101"
            };
        }

        private static void CrearFactura()
        {
            try
            {
                Console.WriteLine("Ejemplo Factura Gravada (FF11-001)");
                var documento = new DocumentoElectronico
                {
                    Emisor = CrearEmisor(),
                    Receptor = new Compania
                    {
                        NroDocumento = "20100039207",
                        TipoDocumento = "6",
                        NombreLegal = "RANSA COMERCIAL S.A."
                    },
                    IdDocumento = "FF11-001",
                    FechaEmision = DateTime.Today.ToString(FormatoFecha),
                    HoraEmision = "12:00:00", //DateTime.Now.ToString("HH:mm:ss"),
                    Moneda = "PEN",
                    TipoDocumento = "01",
                    TotalIgv = 125.7084m,
                    TotalVenta = 824.0884m,
                    Gravadas = 698.38m,
                    Items = new List<DetalleDocumento>
                    {
                        new DetalleDocumento
                        {
                            Id = 1,
                            Cantidad = 2,
                            PrecioReferencial = 21.19m,
                            PrecioUnitario = 21.19m,
                            TipoPrecio = "01",
                            CodigoItem = "1234234",
                            Descripcion = "Arroz Costeño",
                            UnidadMedida = "NIU",
                            Impuesto = 7.62m, //Impuesto del Precio * Cantidad
                            TipoImpuesto = "10", // Gravada
                            TotalVenta = 50m,
                        },
                        new DetalleDocumento
                        {
                            Id = 2,
                            Cantidad = 10,
                            PrecioReferencial = 45.60m,
                            PrecioUnitario = 45.60m,
                            TipoPrecio = "01",
                            CodigoItem = "AER345667",
                            Descripcion = "Aceite Primor",
                            UnidadMedida = "NIU",
                            Impuesto = 82.08m,
                            TipoImpuesto = "10", // Gravada
                            TotalVenta = 538.08m,
                        },
                        new DetalleDocumento
                        {
                            Id = 3,
                            Cantidad = 10,
                            PrecioReferencial = 20,
                            PrecioUnitario = 20,
                            TipoPrecio = "01",
                            CodigoItem = "3445666777",
                            Descripcion = "Shampoo Palmolive",
                            UnidadMedida = "NIU",
                            Impuesto = 36,
                            TipoImpuesto = "10", // Gravada
                            TotalVenta = 236,
                        }
                    }
                };

                FirmaryEnviar(documento, GenerarDocumento(documento));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CrearFacturaCasosCombinados()
        {
            try
            {
                Console.WriteLine("Ejemplo Factura Gravada + Exonrada + Con Bonificacion + Descuento Global (F001-4355)");
                var documento = new DocumentoElectronico
                {
                    Emisor = CrearEmisor(),
                    Receptor = new Compania
                    {
                        NroDocumento = "20100039207",
                        TipoDocumento = "6",
                        NombreLegal = "RANSA COMERCIAL S.A."
                    },
                    IdDocumento = "F001-4355",
                    FechaEmision = DateTime.Today.AddDays(-5).ToString(FormatoFecha),
                    HoraEmision = "12:00:00", //DateTime.Now.ToString("HH:mm:ss"),
                    Moneda = "PEN",
                    TipoDocumento = "01",
                    TotalIgv = 62675.85m,
                    TotalVenta = 423225.00m,
                    Gravadas = 348199.15m,
                    Exoneradas = 12350.00m,
                    Inafectas = 0m,
                    DescuentoGlobal = 18976.27m,
                    Items = new List<DetalleDocumento>
                    {
                        new DetalleDocumento
                        {
                            Id = 1,
                            Cantidad = 2000,
                            PrecioReferencial = 149491.53m,
                            PrecioUnitario = 83.05M,
                            TipoPrecio = "01",
                            CodigoItem = "GLG199",
                            Descripcion = "Grabadora LG Externo Modelo: GE20LU10",
                            UnidadMedida = "NIU",
                            Impuesto = 29898m,
                            TipoImpuesto = "10",
                            TotalVenta = 176400m,
                            Descuento = 16610.17m,
                        },
                        new DetalleDocumento
                        {
                            Id = 2,
                            Cantidad = 300,
                            PrecioReferencial = 133983.05m,
                            PrecioUnitario = 525.42m,
                            TipoPrecio = "01",
                            CodigoItem = "MVS546",
                            Descripcion = "Monitor LCD ViewSonic VG2028WM 20",
                            UnidadMedida = "NIU",
                            Impuesto = 28372.68m, //24116.95m,
                            TipoImpuesto = "10",
                            TotalVenta = 158100m,
                            Descuento = 23644.07m,
                        },
                        new DetalleDocumento
                        {
                            Id = 3,
                            Cantidad = 250,
                            PrecioReferencial = 13000.00m,
                            PrecioUnitario = 52m,
                            TipoPrecio = "01",
                            CodigoItem = "MPC35",
                            Descripcion = "Memoria DDR-3 B1333 Kingston",
                            UnidadMedida = "NIU",
                            Impuesto = 0,
                            TipoImpuesto = "30",
                            TotalVenta = 13000m,
                            Descuento = 0,
                        },
                        new DetalleDocumento
                        {
                            Id = 4,
                            Cantidad = 500,
                            PrecioReferencial = 83050.85m,
                            PrecioUnitario = 166.10m,
                            TipoPrecio = "01",
                            CodigoItem = "TMS22",
                            Descripcion = "Teclado Microsoft SideWinder X6",
                            UnidadMedida = "NIU",
                            Impuesto = 14949m,
                            TipoImpuesto = "10",
                            TotalVenta = 98000m,
                            Descuento = 0,
                        },
                        new DetalleDocumento
                        {
                            Id = 5,
                            Cantidad = 1,
                            PrecioReferencial = 30m,
                            PrecioUnitario = 0m,
                            TipoPrecio = "02",
                            CodigoItem = "WCG01",
                            Descripcion = "Web cam Genius iSlim 310",
                            UnidadMedida = "NIU",
                            Impuesto = 0,
                            TipoImpuesto = "21",
                            TotalVenta = 0,
                            Descuento = 0,
                        }
                    }
                };

                FirmaryEnviar(documento, GenerarDocumento(documento));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }

        }

        private static void CrearFacturaConDescuento()
        {
            try
            {
                Console.WriteLine("Ejemplo Factura con Descuento y Orden de Compra (FF30-001)");
                var documento = new DocumentoElectronico
                {
                    Emisor = CrearEmisor(),
                    Receptor = new Compania
                    {
                        NroDocumento = "20100039207",
                        TipoDocumento = "6",
                        NombreLegal = "RANSA COMERCIAL S.A."
                    },
                    IdDocumento = "FF30-001",
                    //NroOrdenCompra = "OC-0442",
                    FechaEmision = DateTime.Today.ToString(FormatoFecha),
                    HoraEmision = "12:00:00", //DateTime.Now.ToString("HH:mm:ss"),
                    Moneda = "PEN",
                    TipoDocumento = "01",
                    TotalIgv = 206.91m,
                    TotalVenta = 1347.09m,
                    Gravadas = 1149.5m, //Sumatoria de todas las operaciones gravadas
                    DescuentoGlobal = 9.32m, //sumatoria de todos los descuentos
                    CodigoRazonDcto = "00",
                    FactorMultiplicadorDscto = 69,
                    LineExtensionAmount = 1149.5m,
                    Items = new List<DetalleDocumento>
                    {
                        new DetalleDocumento
                        {
                            Id = 1,
                            Cantidad = 110,
                            PrecioReferencial = 10.45m, //Princing Reference
                            PrecioUnitario = 10.45m, // Price Amount 
                            TipoPrecio = "01",
                            CodigoItem = "D2",
                            Descripcion = "DIESEL B5 S-50 UV",
                            UnidadMedida = "GLL",
                            Impuesto = 206.91m, // ((BASE IMPONIBLE = 1149.5) * 0.18
                            TipoImpuesto = "10", // Gravada
                            TotalVenta = 1356.41m, // LineExtensionAmount (PU * CANTIDAD) + IGV - DESCTO.
                        }
                    }
                };

                FirmaryEnviar(documento, GenerarDocumento(documento));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CrearFacturaExonerada()
        {
            try
            {
                Console.WriteLine("Ejemplo Factura Exonerada (FF11-002)");
                var documento = new DocumentoElectronico
                {
                    Emisor = CrearEmisor(),
                    Receptor = new Compania
                    {
                        NroDocumento = "20100039207",
                        TipoDocumento = "6",
                        NombreLegal = "RANSA COMERCIAL S.A."
                    },
                    IdDocumento = "FF11-002",
                    FechaEmision = DateTime.Today.ToString(FormatoFecha),
                    HoraEmision = "12:00:00", // DateTime.Now.ToString("HH:mm:ss"),
                    Moneda = "PEN",
                    TipoDocumento = "01",
                    TotalIgv = 0,
                    TotalVenta = 30,
                    Exoneradas = 30,
                    Items = new List<DetalleDocumento>
                    {
                        new DetalleDocumento
                        {
                            Id = 1,
                            Cantidad = 1,
                            PrecioReferencial = 30,
                            PrecioUnitario = 30,
                            TipoPrecio = "01",
                            CodigoItem = "1234234",
                            Descripcion = "MATRICULA",
                            UnidadMedida = "NIU",
                            Impuesto = 0,
                            TipoImpuesto = "20", // Exonerada
                            TotalVenta = 30,
                        }
                    }
                };

                FirmaryEnviar(documento, GenerarDocumento(documento));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CrearFacturaExportacion()
        {
            try
            {
                Console.WriteLine("Ejemplo Factura a No Domiciliado (FF00-02456748)");
                var documento = new DocumentoElectronico
                {
                    Emisor = CrearEmisor(),
                    Receptor = new Compania
                    {
                        NroDocumento = "99999999",
                        TipoDocumento = "0",
                        NombreLegal = "EMPRESA EXTRANJERA S.A."
                    },
                    IdDocumento = "FF00-02456748",
                    FechaEmision = DateTime.Today.ToString(FormatoFecha),
                    HoraEmision = "12:00:00", // DateTime.Now.ToString("HH:mm:ss"),
                    Moneda = "PEN",
                    TipoDocumento = "01",
                    TipoOperacion = "0401",
                    TotalIgv = 0,
                    TotalVenta = 2000,
                    Exoneradas = 2000,
                    CodigoBienOServicio = "012",
                    Items = new List<DetalleDocumento>
                    {
                        new DetalleDocumento
                        {
                            Id = 1,
                            Cantidad = 1,
                            PrecioReferencial = 2000,
                            PrecioUnitario = 2000,
                            TipoPrecio = "01",
                            CodigoItem = "KIUO3088078",
                            Descripcion = "Servicio Empresarial",
                            UnidadMedida = "ZZ",
                            Impuesto = 0,
                            TipoImpuesto = "40", // Exportacion
                            TotalVenta = 2000,
                            CodigoProductoSunat = "43230000"
                        }
                    }
                };

                FirmaryEnviar(documento, GenerarDocumento(documento));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CrearFacturaInafecta()
        {
            try
            {
                Console.WriteLine("Ejemplo Factura Inafecta (FF11-003)");
                var documento = new DocumentoElectronico
                {
                    Emisor = CrearEmisor(),
                    Receptor = new Compania
                    {
                        NroDocumento = "20100039207",
                        TipoDocumento = "6",
                        NombreLegal = "RANSA COMERCIAL S.A."
                    },
                    IdDocumento = "FF11-003",
                    FechaEmision = DateTime.Today.ToString(FormatoFecha),
                    HoraEmision = "12:30:00", //DateTime.Now.ToString("HH:mm:ss"),
                    Moneda = "PEN",
                    TipoDocumento = "01",
                    TotalIgv = 0,
                    TotalVenta = 468.60m,
                    Inafectas = 468.60m,
                    Items = new List<DetalleDocumento>
                    {
                        new DetalleDocumento
                        {
                            Id = 1,
                            Cantidad = 100,
                            PrecioReferencial = 4.20m,
                            PrecioUnitario = 4.20m,
                            TipoPrecio = "01",
                            CodigoItem = "1675",
                            Descripcion = "HUEVOS PARDOS (GRANEL)",
                            UnidadMedida = "NIU",
                            Impuesto = 0,
                            TipoImpuesto = "30", // Inafecta
                            TotalVenta = 420,
                        },new DetalleDocumento
                        {
                            Id = 2,
                            Cantidad = 15,
                            PrecioReferencial = 3.24m,
                            PrecioUnitario = 3.24m,
                            TipoPrecio = "01",
                            CodigoItem = "1676",
                            Descripcion = "HUEVITOS DE CODORNIZ MALLA X 25",
                            UnidadMedida = "NIU",
                            Impuesto = 0,
                            TipoImpuesto = "30", // Inafecta
                            TotalVenta = 48.60m,
                        },
                    }
                };

                FirmaryEnviar(documento, GenerarDocumento(documento));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CrearFacturaConPlaca()
        {
            try
            {
                Console.WriteLine("Ejemplo Factura con Placa Vehicular (FF13-001)");
                var documento = new DocumentoElectronico
                {
                    Emisor = CrearEmisor(),
                    Receptor = new Compania
                    {
                        NroDocumento = "20100039207",
                        TipoDocumento = "6",
                        NombreLegal = "RANSA COMERCIAL S.A."
                    },
                    IdDocumento = "FF13-001",
                    FechaEmision = DateTime.Today.AddDays(-5).ToString(FormatoFecha),
                    HoraEmision = DateTime.Now.ToString("HH:mm:ss"),
                    Moneda = "PEN",
                    TipoDocumento = "01",
                    TotalIgv = 16.8m,
                    TotalVenta = 76.8m,
                    Gravadas = 60,
                    Items = new List<DetalleDocumento>
                    {
                        new DetalleDocumento
                        {
                            Id = 1,
                            Cantidad = 15,
                            PrecioReferencial = 4,
                            PrecioUnitario = 4,
                            TipoPrecio = "01",
                            CodigoItem = "GAS-01",
                            Descripcion = "GASOLINA 95",
                            UnidadMedida = "GLI",
                            Impuesto = 10.8m,
                            TipoImpuesto = "10", // Gravada
                            TotalVenta = 60,
                            PlacaVehiculo = "YG-9244"
                        }
                    }
                };

                FirmaryEnviar(documento, GenerarDocumento(documento));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CrearFacturaConDetraccion()
        {
            try
            {
                Console.WriteLine("Ejemplo Factura con Detracción (FF12-500)");
                var documento = new DocumentoElectronico
                {
                    Emisor = CrearEmisor(),
                    Receptor = new Compania
                    {
                        NroDocumento = "20565211600",
                        TipoDocumento = "6",
                        NombreLegal = "WASPE PERU S.A.C."
                    },
                    IdDocumento = "FF12-500",
                    FechaEmision = DateTime.Today.ToString(FormatoFecha),
                    HoraEmision = DateTime.Now.ToString("HH:mm:ss"),
                    FechaVencimiento = DateTime.Today.AddDays(3).ToString(FormatoFecha),
                    Moneda = "PEN",
                    TipoDocumento = "01",
                    TipoOperacion = "1001",
                    CuentaBancoNacion = "00047-345",
                    MontoDetraccion = 99.12m,
                    TasaDetraccion = 12, //12% de Detraccion
                    CodigoBienOServicio = "022",  //Otros servicios empresariales (Catalogo 54)
                    CodigoMedioPago = "001", // Medio de Pago (Catalogo 59)
                    TotalIgv = 126,
                    TotalVenta = 826,
                    Gravadas = 700,
                    Items = new List<DetalleDocumento>
                    {
                        new DetalleDocumento
                        {
                            Id = 1,
                            Cantidad = 1,
                            PrecioReferencial = 700,
                            PrecioUnitario = 700,
                            TipoPrecio = "01",
                            CodigoItem = "DES-02",
                            Descripcion = "OPENINVOICEPERU UBL 2.1",
                            UnidadMedida = "NIU",
                            Impuesto = 126,
                            TipoImpuesto = "10", // Gravada
                            TotalVenta = 700 // Monto sin IGV
                        }
                    },
                    Leyendas = new List<Leyenda>
                    {
                        new Leyenda
                        {
                            Codigo = "2006",
                            Descripcion = "Operación sujeta a detracción"
                        }
                    }
                };

                FirmaryEnviar(documento, GenerarDocumento(documento));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CrearFacturaConDetraccionTransportes()
        {
            try
            {
                Console.WriteLine("Ejemplo Factura con Detracción de Transportes (FF80-001)");
                var documento = new DocumentoElectronico
                {
                    Emisor = CrearEmisor(),
                    Receptor = new Compania
                    {
                        NroDocumento = "20100039207",
                        TipoDocumento = "6",
                        NombreLegal = "RANSA COMERCIAL S.A."
                    },
                    IdDocumento = "FF80-001",
                    FechaEmision = DateTime.Today.AddDays(-5).ToString(FormatoFecha),
                    HoraEmision = DateTime.Now.ToString("HH:mm:ss"),
                    Moneda = "PEN",
                    TipoDocumento = "01",
                    TipoOperacion = "1001",
                    CuentaBancoNacion = "00047-345",
                    MontoDetraccion = 99.12m,
                    TasaDetraccion = 4, //4% de Detracción
                    CodigoBienOServicio = "027",  //Servicio de Transporte de Carga (Catalogo 54)
                    CodigoMedioPago = "001", // Medio de Pago (Catalogo 59)
                    TotalIgv = 18,
                    TotalVenta = 118,
                    Gravadas = 100,
                    Items = new List<DetalleDocumento>
                    {
                        new DetalleDocumento
                        {
                            Id = 1,
                            Cantidad = 5,
                            PrecioReferencial = 20,
                            PrecioUnitario = 20,
                            TipoPrecio = "01",
                            CodigoItem = "1234234",
                            Descripcion = "Transporte",
                            UnidadMedida = "KG",
                            Impuesto = 18,
                            TipoImpuesto = "10", // Gravada
                            TotalVenta = 100,
                            UbigeoOrigen = "150101",
                            DireccionOrigen = "Av. Argentina 2388",
                            UbigeoDestino = "160101",
                            DireccionDestino = "Jr. Morona 171",
                            DetalleViaje = "Viaje con carga pesada",
                            ValorReferencial = 500,
                            ValorReferencialCargaEfectiva = 520,
                            ValorReferencialCargaUtil = 480
                        }
                    }
                };

                FirmaryEnviar(documento, GenerarDocumento(documento));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CrearFacturaConAnticipo()
        {
            try
            {
                Console.WriteLine("Ejemplo Factura de Anticipo Y Regularización del mismo (FF60-1500 y FF60-1501)");
                var documento = new DocumentoElectronico
                {
                    Emisor = CrearEmisor(),
                    Receptor = new Compania
                    {
                        NroDocumento = "20565211600",
                        TipoDocumento = "6",
                        NombreLegal = "WASPE PERU S.A.C."
                    },
                    IdDocumento = "FF60-1500",
                    FechaEmision = DateTime.Today.ToString(FormatoFecha),
                    HoraEmision = DateTime.Now.ToString("HH:mm:ss"),
                    FechaVencimiento = DateTime.Today.AddDays(7).ToString(FormatoFecha),
                    Moneda = "PEN",
                    TipoDocumento = "01",
                    TipoOperacion = "0101",
                    TotalIgv = 126,
                    TotalVenta = 826,
                    Gravadas = 700,
                    Items = new List<DetalleDocumento>
                    {
                        new DetalleDocumento
                        {
                            Id = 1,
                            Cantidad = 1,
                            PrecioReferencial = 700,
                            PrecioUnitario = 700,
                            TipoPrecio = "01",
                            CodigoItem = "DES-02",
                            Descripcion = "OPENINVOICEPERU UBL 2.1 ANTICIPO 50%",
                            UnidadMedida = "NIU",
                            Impuesto = 126,
                            TipoImpuesto = "10", // Gravada
                            TotalVenta = 826 // (PrecioUnitario * Cantidad) + IGV
                        }
                    }
                };

                var documentoRegularizador = new DocumentoElectronico
                {
                    Emisor = CrearEmisor(),
                    Receptor = new Compania
                    {
                        NroDocumento = "20565211600",
                        TipoDocumento = "6",
                        NombreLegal = "WASPE PERU S.A.C."
                    },
                    IdDocumento = "FF60-1501",
                    FechaEmision = DateTime.Today.ToString(FormatoFecha),
                    HoraEmision = DateTime.Now.ToString("HH:mm:ss"),
                    Moneda = "PEN",
                    TipoDocumento = "01",
                    TipoOperacion = "0101",
                    Anticipos = new List<Anticipo>
                    {
                        new Anticipo
                        {
                            DocAnticipo = "FF60-1500", //Especificamos el Documento Previo
                            MonedaAnticipo = "PEN", //Moneda del Documento Anterior
                            MontoAnticipo = 826, //Monto Pagado previamente
                            TipoDocAnticipo = "02", // Tipo de Documento del Anticipo (Catalogo 12),
                        }
                    },
                    MontoTotalAnticipo = 826,
                    TotalIgv = 126,
                    TotalVenta = 826,
                    Gravadas = 700,
                    Items = new List<DetalleDocumento>
                    {
                        new DetalleDocumento
                        {
                            Id = 1,
                            Cantidad = 1,
                            PrecioReferencial = 700,
                            PrecioUnitario = 700,
                            TipoPrecio = "01",
                            CodigoItem = "DES-02",
                            Descripcion = "OPENINVOICEPERU UBL 2.1 REGULARIZACIÓN 50%",
                            UnidadMedida = "NIU",
                            Impuesto = 126,
                            TipoImpuesto = "10", // Gravada
                            TotalVenta = 826 // (PrecioUnitario * Cantidad) + IGV
                        }
                    }
                };

                FirmaryEnviar(documento, GenerarDocumento(documento));

                FirmaryEnviar(documentoRegularizador, GenerarDocumento(documentoRegularizador));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CrearFacturaGratuita()
        {
            try
            {
                Console.WriteLine("Ejemplo Factura Gratuita (FF20-005)");
                var documento = new DocumentoElectronico
                {
                    Emisor = CrearEmisor(),
                    Receptor = new Compania
                    {
                        NroDocumento = "20100039207",
                        TipoDocumento = "6",
                        NombreLegal = "RANSA COMERCIAL S.A."
                    },
                    IdDocumento = "FF20-005",
                    FechaEmision = DateTime.Today.ToString(FormatoFecha),
                    HoraEmision = "12:00:00", //DateTime.Now.ToString("HH:mm:ss"),
                    Moneda = "PEN",
                    TipoDocumento = "01",
                    TotalIgv = 0,
                    TotalVenta = 0,
                    Gratuitas = 60,
                    Items = new List<DetalleDocumento>
                    {
                        new DetalleDocumento
                        {
                            Id = 1,
                            Cantidad = 1,
                            PrecioReferencial = 60,
                            PrecioUnitario = 0,
                            TipoPrecio = "01",
                            CodigoItem = "1234234",
                            Descripcion = "CORREA",
                            UnidadMedida = "NIU",
                            Impuesto = 0,
                            TipoImpuesto = "21", // Gratuita, Promocion, Retiro, Donacion
                            TotalVenta = 0,
                        }
                    }
                };

                FirmaryEnviar(documento, GenerarDocumento(documento));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CrearBoleta()
        {
            try
            {
                Console.WriteLine("Ejemplo Boleta (BB11-001)");
                var documento = new DocumentoElectronico
                {
                    Emisor = CrearEmisor(),
                    Receptor = new Compania
                    {
                        NroDocumento = "88888888",
                        TipoDocumento = "1",
                        NombreLegal = "CLIENTE GENERICO"
                    },
                    IdDocumento = "BB11-001",
                    FechaEmision = DateTime.Today.AddDays(-5).ToString(FormatoFecha),
                    HoraEmision = DateTime.Now.ToString("HH:mm:ss"),
                    Moneda = "USD",
                    TipoDocumento = "03",
                    TotalIgv = 18,
                    TotalVenta = 118,
                    Gravadas = 100,
                    Items = new List<DetalleDocumento>
                    {
                        new DetalleDocumento
                        {
                            Id = 1,
                            Cantidad = 10,
                            PrecioReferencial = 10,
                            PrecioUnitario = 10,
                            TipoPrecio = "01",
                            CodigoItem = "CSHARP-01",
                            Descripcion = "Desarrollo de Software C#",
                            UnidadMedida = "NIU",
                            Impuesto = 18,
                            TipoImpuesto = "10", // Gravada
                            TotalVenta = 100,
                            CodigoProductoSunat = "81111612"
                        }
                    }
                };

                FirmaryEnviar(documento, GenerarDocumento(documento));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CrearNotaCredito()
        {
            try
            {
                Console.WriteLine("Ejemplo Nota de Crédito de Factura (FN11-001)");
                var documento = new DocumentoElectronico
                {
                    Emisor = CrearEmisor(),
                    Receptor = new Compania
                    {
                        NroDocumento = "20549202960",
                        TipoDocumento = "6",
                        NombreLegal = "EMPRESA DE SOFT",
                        CodigoAnexo = ""
                    },
                    IdDocumento = "FN11-001",
                    FechaEmision = DateTime.Today.AddDays(-5).ToString(FormatoFecha),
                    HoraEmision = DateTime.Now.ToString("HH:mm:ss"),
                    MontoEnLetras = string.Empty,
                    Moneda = "PEN",
                    TipoDocumento = "07",
                    TotalIgv = 3.97m,
                    TotalVenta = 26.04m,
                    Gravadas = 22.07m,
                    Items = new List<DetalleDocumento>
                    {
                        new DetalleDocumento
                        {
                            Id = 1,
                            Cantidad = 1,
                            PrecioReferencial = 22.07m,
                            PrecioUnitario = 22.07m,
                            TipoPrecio = "01",
                            CodigoItem = "2435675",
                            Descripcion = "Corrección Factura",
                            UnidadMedida = "NIU",
                            Impuesto = 3.97m,
                            TipoImpuesto = "10", // Gravada
                            TotalVenta = 22.07m,
                        }
                    },
                    Discrepancias = new List<Discrepancia>
                    {
                        new Discrepancia
                        {
                            NroReferencia = "FF11-001",
                            Tipo = "01",
                            Descripcion = "Anulacion de la operacion"
                        }
                    },
                    Relacionados = new List<DocumentoRelacionado>
                    {
                        new DocumentoRelacionado
                        {
                            NroDocumento = "FF11-001",
                            TipoDocumento = "01"
                        }
                    }
                };

                File.WriteAllText("notacredito.json", Newtonsoft.Json.JsonConvert.SerializeObject(documento));

                FirmaryEnviar(documento, GenerarDocumento(documento));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CrearNotaDebito()
        {
            try
            {
                Console.WriteLine("Ejemplo Nota de Débito de Factura (FD11-001)");
                var documento = new DocumentoElectronico
                {
                    Emisor = CrearEmisor(),
                    Receptor = new Compania
                    {
                        NroDocumento = "20257471609",
                        TipoDocumento = "6",
                        NombreLegal = "FRAMEWORK PERU"
                    },
                    IdDocumento = "FD11-001",
                    FechaEmision = DateTime.Today.ToString(FormatoFecha),
                    HoraEmision = DateTime.Now.ToString("HH:mm:ss"),
                    Moneda = "PEN",
                    TipoDocumento = "08",
                    TotalIgv = 0.76m,
                    TotalVenta = 5,
                    Gravadas = 4.24m,
                    Items = new List<DetalleDocumento>
                    {
                        new DetalleDocumento
                        {
                            Id = 1,
                            Cantidad = 1,
                            PrecioReferencial = 4.24m,
                            PrecioUnitario = 4.24m,
                            TipoPrecio = "01",
                            CodigoItem = "2435675",
                            Descripcion = "Penalidad por atraso de pago",
                            UnidadMedida = "NIU",
                            Impuesto = 0.76m,
                            TipoImpuesto = "10", // Gravada
                            TotalVenta = 5,
                        }
                    },
                    Discrepancias = new List<Discrepancia>
                    {
                        new Discrepancia
                        {
                            NroReferencia = "FF11-001",
                            Tipo = "03",
                            Descripcion = "Penalidad por falta de pago"
                        }
                    },
                    Relacionados = new List<DocumentoRelacionado>
                    {
                        new DocumentoRelacionado
                        {
                            NroDocumento = "FF11-001",
                            TipoDocumento = "01"
                        }
                    }
                };

                FirmaryEnviar(documento, GenerarDocumento(documento));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CrearResumenDiario()
        {
            try
            {
                Console.WriteLine("Ejemplo de Resumen Diario");
                var documentoResumenDiario = new ResumenDiarioNuevo
                {
                    IdDocumento = $"RC-{DateTime.Today:yyyyMMdd}-001",
                    FechaEmision = DateTime.Today.ToString(FormatoFecha),
                    FechaReferencia = DateTime.Today.AddDays(-1).ToString(FormatoFecha),
                    Emisor = CrearEmisor(),
                    Resumenes = new List<GrupoResumenNuevo>()
                };

                documentoResumenDiario.Resumenes.Add(new GrupoResumenNuevo
                {
                    Id = 1,
                    TipoDocumento = "03",
                    IdDocumento = "BB14-33386",
                    NroDocumentoReceptor = "41614074",
                    TipoDocumentoReceptor = "1",
                    CodigoEstadoItem = 1, // 1 - Agregar. 2 - Modificar. 3 - Eliminar
                    Moneda = "PEN",
                    TotalVenta = 190.9m,
                    TotalIgv = 29.12m,
                    Gravadas = 161.78m,
                });
                // Para los casos de envio de boletas anuladas, se debe primero informar las boletas creadas (1) y luego en un segundo resumen se envian las anuladas. De lo contrario se presentará el error 'El documento indicado no existe no puede ser modificado/eliminado'
                documentoResumenDiario.Resumenes.Add(new GrupoResumenNuevo
                {
                    Id = 2,
                    TipoDocumento = "03",
                    IdDocumento = "BB30-33384",
                    NroDocumentoReceptor = "08506678",
                    TipoDocumentoReceptor = "1",
                    CodigoEstadoItem = 1, // 1 - Agregar. 2 - Modificar. 3 - Eliminar
                    Moneda = "USD",
                    TotalVenta = 9580m,
                    TotalIgv = 1411.36m,
                    Gravadas = 8168.64m,
                });


                Console.WriteLine("Generando XML....");

                var documentoResponse = RestHelper<ResumenDiarioNuevo, DocumentoResponse>.Execute("GenerarResumenDiario/v2", documentoResumenDiario);

                if (!documentoResponse.Exito)
                    throw new InvalidOperationException(documentoResponse.MensajeError);

                Console.WriteLine("Firmando XML...");
                // Firmado del Documento.
                var firmado = new FirmadoRequest
                {
                    TramaXmlSinFirma = documentoResponse.TramaXmlSinFirma,
                    CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes("Certificado.pfx")),
                    PasswordCertificado = string.Empty,
                };

                var responseFirma = RestHelper<FirmadoRequest, FirmadoResponse>.Execute("Firmar", firmado);

                if (!responseFirma.Exito)
                {
                    throw new InvalidOperationException(responseFirma.MensajeError);
                }

                Console.WriteLine("Guardando XML de Resumen....(Revisar carpeta del ejecutable)");

                File.WriteAllBytes("resumendiario.xml", Convert.FromBase64String(responseFirma.TramaXmlFirmado));

                Console.WriteLine("Enviando a SUNAT....");

                var enviarDocumentoRequest = new EnviarDocumentoRequest
                {
                    Ruc = documentoResumenDiario.Emisor.NroDocumento,
                    UsuarioSol = "MODDATOS",
                    ClaveSol = "MODDATOS",
                    EndPointUrl = UrlSunat,
                    IdDocumento = documentoResumenDiario.IdDocumento,
                    TramaXmlFirmado = responseFirma.TramaXmlFirmado
                };

                var enviarResumenResponse = RestHelper<EnviarDocumentoRequest, EnviarResumenResponse>.Execute("EnviarResumen", enviarDocumentoRequest);

                if (!enviarResumenResponse.Exito)
                {
                    throw new InvalidOperationException(enviarResumenResponse.MensajeError);
                }

                Console.WriteLine("Nro de Ticket: {0}", enviarResumenResponse.NroTicket);

                ConsultarTicket(enviarResumenResponse.NroTicket, documentoResumenDiario.Emisor.NroDocumento);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CrearComunicacionBaja()
        {
            try
            {
                Console.WriteLine("Ejemplo de Comunicación de Baja");
                var documentoBaja = new ComunicacionBaja
                {
                    IdDocumento = $"RA-{DateTime.Today:yyyyMMdd}-001",
                    FechaEmision = DateTime.Today.ToString(FormatoFecha),
                    FechaReferencia = DateTime.Today.AddDays(-1).ToString(FormatoFecha),
                    Emisor = CrearEmisor(),
                    Bajas = new List<DocumentoBaja>()
                };

                // En las comunicaciones de Baja ya no se pueden colocar boletas, ya que la anulacion de las mismas
                // la realiza el resumen diario.
                documentoBaja.Bajas.Add(new DocumentoBaja
                {
                    Id = 1,
                    Correlativo = "33386",
                    TipoDocumento = "01",
                    Serie = "FA50",
                    MotivoBaja = "Anulación por otro tipo de documento"
                });
                documentoBaja.Bajas.Add(new DocumentoBaja
                {
                    Id = 2,
                    Correlativo = "86486",
                    TipoDocumento = "01",
                    Serie = "FF14",
                    MotivoBaja = "Anulación por otro datos erroneos"
                });

                Console.WriteLine("Generando XML....");

                var documentoResponse = RestHelper<ComunicacionBaja, DocumentoResponse>.Execute("GenerarComunicacionBaja", documentoBaja);
                if (!documentoResponse.Exito)
                {
                    throw new InvalidOperationException(documentoResponse.MensajeError);
                }

                Console.WriteLine("Firmando XML...");
                // Firmado del Documento.
                var firmado = new FirmadoRequest
                {
                    TramaXmlSinFirma = documentoResponse.TramaXmlSinFirma,
                    CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes("Certificado.pfx")),
                    PasswordCertificado = string.Empty,
                };

                var responseFirma = RestHelper<FirmadoRequest, FirmadoResponse>.Execute("Firmar", firmado);

                if (!responseFirma.Exito)
                {
                    throw new InvalidOperationException(responseFirma.MensajeError);
                }

                Console.WriteLine("Guardando XML de la Comunicacion de Baja....(Revisar carpeta del ejecutable)");

                File.WriteAllBytes("comunicacionbaja.xml", Convert.FromBase64String(responseFirma.TramaXmlFirmado));

                Console.WriteLine("Enviando a SUNAT....");

                var sendBill = new EnviarDocumentoRequest
                {
                    Ruc = documentoBaja.Emisor.NroDocumento,
                    UsuarioSol = "MODDATOS",
                    ClaveSol = "MODDATOS",
                    EndPointUrl = UrlSunat,
                    IdDocumento = documentoBaja.IdDocumento,
                    TramaXmlFirmado = responseFirma.TramaXmlFirmado
                };

                var enviarResumenResponse = RestHelper<EnviarDocumentoRequest, EnviarResumenResponse>.Execute("EnviarResumen", sendBill);

                if (!enviarResumenResponse.Exito)
                {
                    throw new InvalidOperationException(enviarResumenResponse.MensajeError);
                }

                Console.WriteLine("Nro de Ticket: {0}", enviarResumenResponse.NroTicket);

                ConsultarTicket(enviarResumenResponse.NroTicket, documentoBaja.Emisor.NroDocumento);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void ConsultarTicket(string nroTicket, string nroRuc)
        {
            var consultarTicketRequest = new ConsultaTicketRequest
            {
                Ruc = nroRuc,
                NroTicket = nroTicket,
                UsuarioSol = "MODDATOS",
                ClaveSol = "MODDATOS",
                EndPointUrl = UrlSunat
            };

            var response = RestHelper<ConsultaTicketRequest, EnviarDocumentoResponse>.Execute("ConsultarTicket", consultarTicketRequest);

            if (!response.Exito)
            {
                Console.WriteLine(response.MensajeError);
                return;
            }

            var archivo = response.NombreArchivo.Replace(".xml", string.Empty);
            Console.WriteLine($"Escribiendo documento en la carpeta del ejecutable... {archivo}");

            File.WriteAllBytes($"{archivo}.zip", Convert.FromBase64String(response.TramaZipCdr));

            Console.WriteLine($"Código: {response.CodigoRespuesta} => {response.MensajeRespuesta}");
        }

        private static void CrearDocumentoRetencion()
        {
            try
            {
                Console.WriteLine("Ejemplo de Retención (R001-123)");
                var documento = new DocumentoRetencion
                {
                    Emisor = ToNegocio(CrearEmisor()),
                    Receptor = new Negocio
                    {
                        NroDocumento = "20100039207",
                        TipoDocumento = "6",
                        NombreLegal = "RANSA COMERCIAL S.A.",
                        Ubigeo = "150101",
                        Direccion = "Av. Argentina 2833",
                        Urbanizacion = "-",
                        Departamento = "CALLAO",
                        Provincia = "CALLAO",
                        Distrito = "CALLAO"
                    },
                    IdDocumento = "R001-123",
                    FechaEmision = DateTime.Today.ToString(FormatoFecha),
                    Moneda = "PEN",
                    RegimenRetencion = "01",
                    TasaRetencion = 3,
                    ImporteTotalRetenido = 300,
                    ImporteTotalPagado = 10000,
                    Observaciones = "Emision de Facturas del periodo Dic. 2016",
                    DocumentosRelacionados = new List<ItemRetencion>
                    {
                        new ItemRetencion
                        {
                            NroDocumento = "E001-457",
                            TipoDocumento = "01",
                            MonedaDocumentoRelacionado = "USD",
                            FechaEmision = DateTime.Today.AddDays(-3).ToString(FormatoFecha),
                            ImporteTotal = 10000,
                            FechaPago = DateTime.Today.ToString(FormatoFecha),
                            NumeroPago = 153,
                            ImporteSinRetencion = 9700,
                            ImporteRetenido = 300,
                            FechaRetencion = DateTime.Today.ToString(FormatoFecha),
                            ImporteTotalNeto = 10000,
                            TipoCambio = 3.41m,
                            FechaTipoCambio = DateTime.Today.ToString(FormatoFecha)
                        }
                    }
                };

                Console.WriteLine("Generando XML....");

                var documentoResponse = RestHelper<DocumentoRetencion, DocumentoResponse>.Execute("GenerarRetencion", documento);

                if (!documentoResponse.Exito)
                {
                    throw new InvalidOperationException(documentoResponse.MensajeError);
                }

                Console.WriteLine("Firmando XML...");
                // Firmado del Documento.
                var firmado = new FirmadoRequest
                {
                    TramaXmlSinFirma = documentoResponse.TramaXmlSinFirma,
                    CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes("certificado.pfx")),
                    PasswordCertificado = string.Empty,
                };

                var responseFirma = RestHelper<FirmadoRequest, FirmadoResponse>.Execute("Firmar", firmado);

                if (!responseFirma.Exito)
                {
                    throw new InvalidOperationException(responseFirma.MensajeError);
                }

                File.WriteAllBytes("retencion.xml", Convert.FromBase64String(responseFirma.TramaXmlFirmado));

                Console.WriteLine("Enviando Retención a SUNAT....");

                var enviarDocumentoRequest = new EnviarDocumentoRequest
                {
                    Ruc = documento.Emisor.NroDocumento,
                    UsuarioSol = "MODDATOS",
                    ClaveSol = "MODDATOS",
                    EndPointUrl = UrlOtroCpe,
                    IdDocumento = documento.IdDocumento,
                    TipoDocumento = "20",
                    TramaXmlFirmado = responseFirma.TramaXmlFirmado
                };

                var enviarDocumentoResponse = RestHelper<EnviarDocumentoRequest, EnviarDocumentoResponse>.Execute("EnviarDocumento", enviarDocumentoRequest);

                if (!enviarDocumentoResponse.Exito)
                {
                    throw new InvalidOperationException(enviarDocumentoResponse.MensajeError);
                }

                Console.WriteLine("Respuesta de SUNAT:");
                Console.WriteLine(enviarDocumentoResponse.MensajeRespuesta);

                File.WriteAllBytes("retencioncdr.zip", Convert.FromBase64String(enviarDocumentoResponse.TramaZipCdr));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CrearDocumentoPercepcion()
        {
            try
            {
                Console.WriteLine("Ejemplo de Percepción (P001-123)");
                var documento = new DocumentoPercepcion
                {
                    Emisor = ToNegocio(CrearEmisor()),
                    Receptor = new Negocio
                    {
                        NroDocumento = "20100039207",
                        TipoDocumento = "6",
                        NombreLegal = "RANSA COMERCIAL S.A.",
                        Ubigeo = "150101",
                        Direccion = "Av. Argentina 2833",
                        Urbanizacion = "-",
                        Departamento = "CALLAO",
                        Provincia = "CALLAO",
                        Distrito = "CALLAO"
                    },
                    IdDocumento = "P001-123",
                    FechaEmision = DateTime.Today.ToString(FormatoFecha),
                    Moneda = "PEN",
                    RegimenPercepcion = "01",
                    TasaPercepcion = 2,
                    ImporteTotalPercibido = 200,
                    ImporteTotalCobrado = 10000,
                    Observaciones = "Emision de Facturas del periodo Dic. 2016",
                    DocumentosRelacionados = new List<ItemPercepcion>
                    {
                        new ItemPercepcion
                        {
                            NroDocumento = "E001-457",
                            TipoDocumento = "01",
                            MonedaDocumentoRelacionado = "USD",
                            FechaEmision = DateTime.Today.AddDays(-3).ToString(FormatoFecha),
                            ImporteTotal = 10000,
                            FechaPago = DateTime.Today.ToString(FormatoFecha),
                            NumeroPago = 153,
                            ImporteSinPercepcion = 9800,
                            ImportePercibido = 200,
                            FechaPercepcion = DateTime.Today.ToString(FormatoFecha),
                            ImporteTotalNeto = 10000,
                            TipoCambio = 3.41m,
                            FechaTipoCambio = DateTime.Today.ToString(FormatoFecha)
                        }
                    }
                };

                Console.WriteLine("Generando XML....");

                var documentoResponse = RestHelper<DocumentoPercepcion, DocumentoResponse>.Execute("GenerarPercepcion", documento);

                if (!documentoResponse.Exito)
                {
                    throw new InvalidOperationException(documentoResponse.MensajeError);
                }

                Console.WriteLine("Firmando XML...");
                // Firmado del Documento.
                var firmado = new FirmadoRequest
                {
                    TramaXmlSinFirma = documentoResponse.TramaXmlSinFirma,
                    CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes("certificado.pfx")),
                    PasswordCertificado = string.Empty,
                };

                var responseFirma = RestHelper<FirmadoRequest, FirmadoResponse>.Execute("Firmar", firmado);

                if (!responseFirma.Exito)
                {
                    throw new InvalidOperationException(responseFirma.MensajeError);
                }

                File.WriteAllBytes("percepcion.xml", Convert.FromBase64String(responseFirma.TramaXmlFirmado));

                Console.WriteLine("Enviando Retención a SUNAT....");

                var sendBill = new EnviarDocumentoRequest
                {
                    Ruc = documento.Emisor.NroDocumento,
                    UsuarioSol = "MODDATOS",
                    ClaveSol = "MODDATOS",
                    EndPointUrl = UrlOtroCpe,
                    IdDocumento = documento.IdDocumento,
                    TipoDocumento = "40",
                    TramaXmlFirmado = responseFirma.TramaXmlFirmado
                };

                var responseSendBill = RestHelper<EnviarDocumentoRequest, EnviarDocumentoResponse>.Execute("EnviarDocumento", sendBill);

                if (!responseSendBill.Exito)
                {
                    throw new InvalidOperationException(responseSendBill.MensajeError);
                }

                Console.WriteLine("Respuesta de SUNAT:");
                Console.WriteLine(responseSendBill.MensajeRespuesta);

                File.WriteAllBytes("percepcioncdr.zip", Convert.FromBase64String(responseSendBill.TramaZipCdr));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CrearGuiaRemision()
        {
            try
            {
                Console.WriteLine("Ejemplo de Guia de Remisión (TAAA-2344)");
                var guia = new GuiaRemision
                {
                    IdDocumento = "TAAA-2344",
                    FechaEmision = DateTime.Today.ToString(FormatoFecha),
                    TipoDocumento = "09",
                    Glosa = "Guia de Prueba",
                    Remitente = CrearEmisor(),
                    Destinatario = new Contribuyente
                    {
                        NroDocumento = "20100039207",
                        TipoDocumento = "6",
                        NombreLegal = "RANSA COMERCIAL S.A.",
                    },
                    ShipmentId = "001",
                    CodigoMotivoTraslado = "02",
                    DescripcionMotivo = "VENTA DIRECTA",
                    Transbordo = false,
                    PesoBrutoTotal = 50,
                    NroPallets = 0,
                    ModalidadTraslado = "01",
                    FechaInicioTraslado = DateTime.Today.ToString(FormatoFecha),
                    RucTransportista = "20257471609",
                    RazonSocialTransportista = "FRAMEWORK PERU",
                    NroPlacaVehiculo = "YG-9244",
                    NroDocumentoConductor = "88888888",
                    DireccionPartida = new Direccion
                    {
                        Ubigeo = "150119",
                        DireccionCompleta = "AV. ARAMBURU 878"
                    },
                    DireccionLlegada = new Direccion
                    {
                        Ubigeo = "150101",
                        DireccionCompleta = "AV. ARGENTINA 2388"
                    },
                    NumeroContenedor = string.Empty,
                    CodigoPuerto = string.Empty,
                    BienesATransportar = new List<DetalleGuia>()
                {
                    new DetalleGuia
                    {
                        Correlativo = 1,
                        CodigoItem = "XXXX",
                        Descripcion = "XXXXXXX",
                        UnidadMedida = "NIU",
                        Cantidad = 4,
                        LineaReferencia = 1
                    }
                }
                };

                Console.WriteLine("Generando XML....");

                var documentoResponse = RestHelper<GuiaRemision, DocumentoResponse>.Execute("GenerarGuiaRemision", guia);

                if (!documentoResponse.Exito)
                {
                    throw new InvalidOperationException(documentoResponse.MensajeError);
                }

                Console.WriteLine("Firmando XML...");
                // Firmado del Documento.
                var firmado = new FirmadoRequest
                {
                    TramaXmlSinFirma = documentoResponse.TramaXmlSinFirma,
                    CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes("certificado.pfx")),
                    PasswordCertificado = string.Empty,
                };


                var responseFirma = RestHelper<FirmadoRequest, FirmadoResponse>.Execute("Firmar", firmado);

                if (!responseFirma.Exito)
                {
                    throw new InvalidOperationException(responseFirma.MensajeError);
                }

                File.WriteAllBytes("GuiaRemision.xml", Convert.FromBase64String(responseFirma.TramaXmlFirmado));

                Console.WriteLine("Enviando a SUNAT....");


                var documentoRequest = new EnviarDocumentoRequest
                {
                    Ruc = guia.Remitente.NroDocumento,
                    UsuarioSol = "MODDATOS",
                    ClaveSol = "MODDATOS",
                    EndPointUrl = UrlGuiaRemision,
                    IdDocumento = guia.IdDocumento,
                    TipoDocumento = guia.TipoDocumento,
                    TramaXmlFirmado = responseFirma.TramaXmlFirmado
                };

                var enviarDocumentoResponse = RestHelper<EnviarDocumentoRequest, EnviarDocumentoResponse>.Execute("EnviarDocumento", documentoRequest);

                if (!enviarDocumentoResponse.Exito)
                {
                    throw new InvalidOperationException(enviarDocumentoResponse.MensajeError);
                }

                File.WriteAllBytes("GuaiRemisionCdr.zip", Convert.FromBase64String(enviarDocumentoResponse.TramaZipCdr));

                Console.WriteLine("Respuesta de SUNAT:");
                Console.WriteLine(enviarDocumentoResponse.MensajeRespuesta);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }

        }

        private static DocumentoResponse GenerarDocumento(DocumentoElectronico documento)
        {
            Console.WriteLine("Generando XML....");

            var metodo = "GenerarFactura";
            switch (documento.TipoDocumento)
            {
                case "01":
                case "03":
                    metodo = "GenerarFactura";
                    break;
                case "07":
                    metodo = "GenerarNotaCredito";
                    break;
                case "08":
                    metodo = "GenerarNotaDebito";
                    break;
            }

            var documentoResponse = RestHelper<DocumentoElectronico, DocumentoResponse>.Execute(metodo, documento);

            if (!documentoResponse.Exito)
            {
                throw new InvalidOperationException(documentoResponse.MensajeError);
            }

            return documentoResponse;
        }

        private static void FirmaryEnviar(DocumentoElectronico documento, DocumentoResponse documentoResponse)
        {
            Console.WriteLine("Firmando XML...");

            // Firmado del Documento.
            var firmado = new FirmadoRequest
            {
                TramaXmlSinFirma = documentoResponse.TramaXmlSinFirma,
                CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes("certificado.pfx")),
                PasswordCertificado = string.Empty,
                ValoresQr = documentoResponse.ValoresParaQr // Despues de Generar el XML usar los valores para generar la trama QR
            };

            var responseFirma = RestHelper<FirmadoRequest, FirmadoResponse>.Execute("Firmar", firmado);

            if (!responseFirma.Exito)
            {
                throw new InvalidOperationException(responseFirma.MensajeError);
            }

            if (!string.IsNullOrEmpty(responseFirma.CodigoQr))
            {
                using (var mem = new MemoryStream(Convert.FromBase64String(responseFirma.CodigoQr)))
                {
                    Console.WriteLine("Guardando Imagen QR para el documento...");
                    var imagen = Image.FromStream(mem);
                    imagen.Save($"{documento.IdDocumento}.png");
                }
            }

            Console.WriteLine("Escribiendo el archivo {0}.xml .....", documento.IdDocumento);

            File.WriteAllBytes($"{documento.IdDocumento}.xml", Convert.FromBase64String(responseFirma.TramaXmlFirmado));

            Console.WriteLine($"Codigo Hash: {responseFirma.ResumenFirma}");

            Process.Start($"{documento.IdDocumento}.xml");

            Console.WriteLine("Enviando a SUNAT....");

            var documentoRequest = new EnviarDocumentoRequest
            {
                Ruc = documento.Emisor.NroDocumento,
                UsuarioSol = "MODDATOS",
                ClaveSol = "MODDATOS",
                EndPointUrl = UrlSunat,
                IdDocumento = documento.IdDocumento,
                TipoDocumento = documento.TipoDocumento,
                TramaXmlFirmado = responseFirma.TramaXmlFirmado
            };

            var enviarDocumentoResponse = RestHelper<EnviarDocumentoRequest, EnviarDocumentoResponse>.Execute("EnviarDocumento", documentoRequest);

            if (!enviarDocumentoResponse.Exito)
            {
                //throw new InvalidOperationException(enviarDocumentoResponse.MensajeError);
                Console.WriteLine(enviarDocumentoResponse.MensajeError);
                return;
            }

            File.WriteAllBytes($"{documento.IdDocumento}.zip", Convert.FromBase64String(enviarDocumentoResponse.TramaZipCdr));

            Console.WriteLine("Respuesta de SUNAT:");
            Console.WriteLine(enviarDocumentoResponse.MensajeRespuesta);

            Process.Start($"{documento.IdDocumento}.zip");
        }

        private static void FirmaryEnviar(string ruta, DocumentoElectronico documento)
        {
            Console.WriteLine("Firmando XML...");

            // Firmado del Documento.
            var firmado = new FirmadoRequest
            {
                TramaXmlSinFirma = Convert.ToBase64String(File.ReadAllBytes(ruta)),
                CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes("certificado.pfx")),
                PasswordCertificado = string.Empty,
            };

            var responseFirma = RestHelper<FirmadoRequest, FirmadoResponse>.Execute("Firmar", firmado);

            if (!responseFirma.Exito)
            {
                throw new InvalidOperationException(responseFirma.MensajeError);
            }

            Console.WriteLine("Escribiendo el archivo {0}.xml .....", documento.IdDocumento);

            File.WriteAllBytes($"{documento.IdDocumento}.xml", Convert.FromBase64String(responseFirma.TramaXmlFirmado));

            Process.Start($"{documento.IdDocumento}.xml");

            Console.WriteLine("Enviando a SUNAT....");

            var documentoRequest = new EnviarDocumentoRequest
            {
                Ruc = documento.Emisor.NroDocumento,
                UsuarioSol = "MODDATOS",
                ClaveSol = "MODDATOS",
                EndPointUrl = UrlSunat,
                IdDocumento = documento.IdDocumento,
                TipoDocumento = documento.TipoDocumento,
                TramaXmlFirmado = responseFirma.TramaXmlFirmado
            };

            var enviarDocumentoResponse = RestHelper<EnviarDocumentoRequest, EnviarDocumentoResponse>.Execute("EnviarDocumento", documentoRequest);

            if (!enviarDocumentoResponse.Exito)
            {
                throw new InvalidOperationException(enviarDocumentoResponse.MensajeError);
            }

            File.WriteAllBytes($"{documento.IdDocumento}.zip", Convert.FromBase64String(enviarDocumentoResponse.TramaZipCdr));

            Console.WriteLine("Respuesta de SUNAT:");
            Console.WriteLine(enviarDocumentoResponse.MensajeRespuesta);

            Process.Start($"{documento.IdDocumento}.zip");
        }

        private static void DescargarComprobante()
        {
            try
            {
                Console.WriteLine("Consulta de Comprobantes Electrónicos (solo Producción)");
                var ruc = LeerLinea("Ingrese su Nro. de RUC");
                var usuario = LeerLinea("Ingrese usuario SOL");
                var clave = LeerLinea("Ingrese Clave SOL");
                var tipoDoc = LeerLinea("Ingrese Codigo Tipo de Documento a Consultar (01, 03, 07 o 08)");
                var serie = LeerLinea("Ingrese Serie Documento a Leer");
                var correlativo = LeerLinea("Ingrese el correlativo del documento sin ceros");

                var consultaConstanciaRequest = new ConsultaConstanciaRequest
                {
                    UsuarioSol = usuario,
                    ClaveSol = clave,
                    TipoDocumento = tipoDoc,
                    Serie = serie,
                    Numero = Convert.ToInt32(correlativo),
                    Ruc = ruc,
                    EndPointUrl = "https://e-factura.sunat.gob.pe/ol-it-wsconscpegem/billConsultService"
                };

                var documentoResponse =
                    RestHelper<ConsultaConstanciaRequest, EnviarDocumentoResponse>.Execute("ConsultarConstancia",
                        consultaConstanciaRequest);

                if (!documentoResponse.Exito)
                {
                    Console.WriteLine(documentoResponse.MensajeError);
                    return;
                }

                var archivo = documentoResponse.NombreArchivo.Replace(".xml", string.Empty);
                Console.WriteLine($"Escribiendo documento en la carpeta del ejecutable... {archivo}");

                File.WriteAllBytes($"{archivo}.zip", Convert.FromBase64String(documentoResponse.TramaZipCdr));

                Console.WriteLine(
                    $"Código: {documentoResponse.CodigoRespuesta} => {documentoResponse.MensajeRespuesta}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }

        }

        private static string LeerLinea(string mensaje)
        {
            Console.WriteLine(mensaje);
            return Console.ReadLine();
        }
    }
}
