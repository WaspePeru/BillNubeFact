Option Strict On
Option Infer On
Option Explicit On

Imports System.IO
Imports OpenInvoicePeru.Comun.Dto.Intercambio
Imports OpenInvoicePeru.Comun.Dto.Modelos


Class Program
    Private Const UrlSunat As String = "https://e-beta.sunat.gob.pe/ol-ti-itcpfegem-beta/billService"
    Private Const UrlOtroCpe As String = "https://e-beta.sunat.gob.pe/ol-ti-itemision-otroscpe-gem-beta/billService"
    Private Const UrlGuiaRemision As String = "https://e-beta.sunat.gob.pe/ol-ti-itemision-guia-gem-beta/billService"
    Private Const FormatoFecha As String = "yyyy-MM-dd"

    Public Shared Sub Main()
        Console.ForegroundColor = ConsoleColor.Green
        Console.Title = "OpenInvoicePeru - Prueba de Envío de Documentos Electrónicos con UBL 2.1"

        CrearFactura()
        CrearFacturaExportacion()
        CrearFacturaConDescuento()
        CrearFacturaExonerada()
        CrearFacturaInafecta()
        CrearFacturaConPlaca()
        CrearFacturaConDetraccion()
        CrearFacturaConDetraccionTransportes()
        CrearFacturaConAnticipo()
        CrearFacturaGratuita()
        CrearBoleta()
        CrearNotaCredito()
        CrearNotaDebito()
        CrearResumenDiario()
        CrearComunicacionBaja()
        CrearDocumentoRetencion()
        CrearDocumentoPercepcion()
        CrearGuiaRemision()
        DescargarComprobante()
    End Sub

    Private Shared Function CrearEmisor() As Compania
        Return New Compania() With {
            .NroDocumento = "20257471609",
            .TipoDocumento = "6",
            .NombreComercial = "FRAMEWORK PERU",
            .NombreLegal = "EMPRESA DE SOFTWARE S.A.C.",
            .CodigoAnexo = "0001"
        }
    End Function

    Private Shared Function ToNegocio(compania As Compania) As Negocio
        Return New Negocio() With {
            .NroDocumento = compania.NroDocumento,
            .TipoDocumento = compania.TipoDocumento,
            .NombreComercial = compania.NombreComercial,
            .NombreLegal = compania.NombreLegal,
            .Distrito = "LIMA",
            .Provincia = "LIMA",
            .Departamento = "LIMA",
            .Direccion = "AV. LIMA 123",
            .Ubigeo = "150101"
        }
    End Function

    Private Shared Sub CrearFactura()
        Try
            Console.WriteLine("Ejemplo Factura Gravada (FF11-001)")
            'DateTime.Now.ToString("HH:mm:ss"),
            ' Gravada
            Dim documento = New DocumentoElectronico() With {
                .Emisor = CrearEmisor(),
                .Receptor = New Compania() With {
                    .NroDocumento = "20100039207",
                    .TipoDocumento = "6",
                    .NombreLegal = "RANSA COMERCIAL S.A."
                },
                .IdDocumento = "FF11-001",
                .FechaEmision = DateTime.Today.AddDays(-5).ToString(FormatoFecha),
                .HoraEmision = "12:00:00",
                .Moneda = "PEN",
                .TipoDocumento = "01",
                .TotalIgv = 0.9D,
                .TotalVenta = 5.9D,
                .Gravadas = 5,
                .Items = New List(Of DetalleDocumento)() From {
                    New DetalleDocumento() With {
                        .Id = 1,
                        .Cantidad = 1,
                        .PrecioReferencial = 5,
                        .PrecioUnitario = 5,
                        .TipoPrecio = "01",
                        .CodigoItem = "1234234",
                        .Descripcion = "Arroz Costeño",
                        .UnidadMedida = "KG",
                        .Impuesto = 0.9D,
                        .TipoImpuesto = "10",
                        .TotalVenta = 5
                    }
                }
            }

            FirmaryEnviar(documento, GenerarDocumento(documento))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try
    End Sub

    Private Shared Sub CrearFacturaConDescuento()
        Try
            Console.WriteLine("Ejemplo Factura con Descuento y Orden de Compra (FF30-001)")
            'DateTime.Now.ToString("HH:mm:ss"),
            ' Gravada
            Dim documento = New DocumentoElectronico() With {
                .Emisor = CrearEmisor(),
                .Receptor = New Compania() With {
                    .NroDocumento = "20100039207",
                    .TipoDocumento = "6",
                    .NombreLegal = "RANSA COMERCIAL S.A."
                },
                .IdDocumento = "FF30-001",
                .NroOrdenCompra = "OC-0442",
                .FechaEmision = DateTime.Today.ToString(FormatoFecha),
                .HoraEmision = "12:00:00",
                .Moneda = "PEN",
                .TipoDocumento = "01",
                .TotalIgv = 0.9D,
                .TotalVenta = 5.9D,
                .Gravadas = 5,
                .DescuentoGlobal = 1,
                .Items = New List(Of DetalleDocumento)() From {
                    New DetalleDocumento() With {
                        .Id = 1,
                        .Cantidad = 1,
                        .PrecioReferencial = 5,
                        .PrecioUnitario = 5,
                        .Descuento = 1,
                        .TipoPrecio = "01",
                        .CodigoItem = "1234234",
                        .Descripcion = "Arroz Costeño",
                        .UnidadMedida = "KG",
                        .Impuesto = 0.9D,
                        .TipoImpuesto = "10",
                        .TotalVenta = 5
                    }
                }
            }

            FirmaryEnviar(documento, GenerarDocumento(documento))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try
    End Sub

    Private Shared Sub CrearFacturaExonerada()
        Try
            Console.WriteLine("Ejemplo Factura Exonerada (FF11-002)")
            ' DateTime.Now.ToString("HH:mm:ss"),
            ' Exonerada
            Dim documento = New DocumentoElectronico() With {
                .Emisor = CrearEmisor(),
                .Receptor = New Compania() With {
                    .NroDocumento = "20100039207",
                    .TipoDocumento = "6",
                    .NombreLegal = "RANSA COMERCIAL S.A."
                },
                .IdDocumento = "FF11-002",
                .FechaEmision = DateTime.Today.ToString(FormatoFecha),
                .HoraEmision = "12:00:00",
                .Moneda = "PEN",
                .TipoDocumento = "01",
                .TotalIgv = 0,
                .TotalVenta = 100,
                .Exoneradas = 100,
                .Items = New List(Of DetalleDocumento)() From {
                    New DetalleDocumento() With {
                        .Id = 1,
                        .Cantidad = 5,
                        .PrecioReferencial = 20,
                        .PrecioUnitario = 20,
                        .TipoPrecio = "01",
                        .CodigoItem = "1234234",
                        .Descripcion = "Arroz Costeño",
                        .UnidadMedida = "KG",
                        .Impuesto = 0,
                        .TipoImpuesto = "20",
                        .TotalVenta = 100
                    }
                }
            }

            FirmaryEnviar(documento, GenerarDocumento(documento))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try
    End Sub

    Private Shared Sub CrearFacturaExportacion()
        Try
            Console.WriteLine("Ejemplo Factura a No Domiciliado (FF00-02456748)")
            ' DateTime.Now.ToString("HH:mm:ss"),
            ' Exportacion
            Dim documento = New DocumentoElectronico() With {
                .Emisor = CrearEmisor(),
                .Receptor = New Compania() With {
                    .NroDocumento = "99999999",
                    .TipoDocumento = "0",
                    .NombreLegal = "EMPRESA EXTRANJERA S.A."
                },
                .IdDocumento = "FF00-02456748",
                .FechaEmision = DateTime.Today.ToString(FormatoFecha),
                .HoraEmision = "12:00:00",
                .Moneda = "PEN",
                .TipoDocumento = "01",
                .TipoOperacion = "0401",
                .TotalIgv = 0,
                .TotalVenta = 2000,
                .Exoneradas = 2000,
                .CodigoBienOServicio = "012",
                .Items = New List(Of DetalleDocumento)() From {
                    New DetalleDocumento() With {
                        .Id = 1,
                        .Cantidad = 1,
                        .PrecioReferencial = 2000,
                        .PrecioUnitario = 2000,
                        .TipoPrecio = "01",
                        .CodigoItem = "KIUO3088078",
                        .Descripcion = "Servicio Empresarial",
                        .UnidadMedida = "ZZ",
                        .Impuesto = 0,
                        .TipoImpuesto = "40",
                        .TotalVenta = 2000,
                        .CodigoProductoSunat = "43230000"
                    }
                }
            }

            FirmaryEnviar(documento, GenerarDocumento(documento))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try
    End Sub

    Private Shared Sub CrearFacturaInafecta()
        Try
            Console.WriteLine("Ejemplo Factura Inafecta (FF11-003)")
            'DateTime.Now.ToString("HH:mm:ss"),
            ' Inafecta
            Dim documento = New DocumentoElectronico() With {
                .Emisor = CrearEmisor(),
                .Receptor = New Compania() With {
                    .NroDocumento = "20100039207",
                    .TipoDocumento = "6",
                    .NombreLegal = "RANSA COMERCIAL S.A."
                },
                .IdDocumento = "FF11-003",
                .FechaEmision = DateTime.Today.ToString(FormatoFecha),
                .HoraEmision = "12:00:00",
                .Moneda = "PEN",
                .TipoDocumento = "01",
                .TotalIgv = 0,
                .TotalVenta = 100,
                .Inafectas = 100,
                .Items = New List(Of DetalleDocumento)() From {
                    New DetalleDocumento() With {
                        .Id = 1,
                        .Cantidad = 5,
                        .PrecioReferencial = 20,
                        .PrecioUnitario = 20,
                        .TipoPrecio = "01",
                        .CodigoItem = "1234234",
                        .Descripcion = "Arroz Costeño",
                        .UnidadMedida = "KG",
                        .Impuesto = 0,
                        .TipoImpuesto = "30",
                        .TotalVenta = 100
                    }
                }
            }

            FirmaryEnviar(documento, GenerarDocumento(documento))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try
    End Sub

    Private Shared Sub CrearFacturaConPlaca()
        Try
            Console.WriteLine("Ejemplo Factura con Placa Vehicular (FF13-001)")
            ' Gravada
            Dim documento = New DocumentoElectronico() With {
                .Emisor = CrearEmisor(),
                .Receptor = New Compania() With {
                    .NroDocumento = "20100039207",
                    .TipoDocumento = "6",
                    .NombreLegal = "RANSA COMERCIAL S.A."
                },
                .IdDocumento = "FF13-001",
                .FechaEmision = DateTime.Today.AddDays(-5).ToString(FormatoFecha),
                .HoraEmision = DateTime.Now.ToString("HH:mm:ss"),
                .Moneda = "PEN",
                .TipoDocumento = "01",
                .TotalIgv = 16.8D,
                .TotalVenta = 76.8D,
                .Gravadas = 60,
                .Items = New List(Of DetalleDocumento)() From {
                    New DetalleDocumento() With {
                        .Id = 1,
                        .Cantidad = 15,
                        .PrecioReferencial = 4,
                        .PrecioUnitario = 4,
                        .TipoPrecio = "01",
                        .CodigoItem = "GAS-01",
                        .Descripcion = "GASOLINA 95",
                        .UnidadMedida = "GLI",
                        .Impuesto = 10.8D,
                        .TipoImpuesto = "10",
                        .TotalVenta = 60,
                        .PlacaVehiculo = "YG-9244"
                    }
                }
            }

            FirmaryEnviar(documento, GenerarDocumento(documento))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try
    End Sub

    Private Shared Sub CrearFacturaConDetraccion()
        Try
            Console.WriteLine("Ejemplo Factura con Detracción (FF12-500)")
            '12% de Detraccion
            'Otros servicios empresariales (Catalogo 54)
            ' Medio de Pago (Catalogo 59)
            ' Gravada
            ' Monto sin IGV
            Dim documento = New DocumentoElectronico() With {
                .Emisor = CrearEmisor(),
                .Receptor = New Compania() With {
                    .NroDocumento = "20565211600",
                    .TipoDocumento = "6",
                    .NombreLegal = "WASPE PERU S.A.C."
                },
                .IdDocumento = "FF12-500",
                .FechaEmision = DateTime.Today.ToString(FormatoFecha),
                .HoraEmision = DateTime.Now.ToString("HH:mm:ss"),
                .FechaVencimiento = DateTime.Today.AddDays(3).ToString(FormatoFecha),
                .Moneda = "PEN",
                .TipoDocumento = "01",
                .TipoOperacion = "1001",
                .CuentaBancoNacion = "00047-345",
                .MontoDetraccion = 99.12D,
                .TasaDetraccion = 12,
                .CodigoBienOServicio = "022",
                .CodigoMedioPago = "001",
                .TotalIgv = 126,
                .TotalVenta = 826,
                .Gravadas = 700,
                .Items = New List(Of DetalleDocumento)() From {
                    New DetalleDocumento() With {
                        .Id = 1,
                        .Cantidad = 1,
                        .PrecioReferencial = 700,
                        .PrecioUnitario = 700,
                        .TipoPrecio = "01",
                        .CodigoItem = "DES-02",
                        .Descripcion = "OPENINVOICEPERU UBL 2.1",
                        .UnidadMedida = "NIU",
                        .Impuesto = 126,
                        .TipoImpuesto = "10",
                        .TotalVenta = 700
                    }
                },
                .Leyendas = New List(Of Leyenda)() From {
                    New Leyenda() With {
                    .Codigo = "2006",
                    .Descripcion = "Operación sujeta a detracción"
                }
                }
        }

            FirmaryEnviar(documento, GenerarDocumento(documento))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try
    End Sub

    Private Shared Sub CrearFacturaConDetraccionTransportes()
        Try
            Console.WriteLine("Ejemplo Factura con Detracción de Transportes (FF80-001)")
            '4% de Detracción
            'Servicio de Transporte de Carga (Catalogo 54)
            ' Medio de Pago (Catalogo 59)
            ' Gravada
            Dim documento = New DocumentoElectronico() With {
                .Emisor = CrearEmisor(),
                .Receptor = New Compania() With {
                    .NroDocumento = "20100039207",
                    .TipoDocumento = "6",
                    .NombreLegal = "RANSA COMERCIAL S.A."
                },
                .IdDocumento = "FF80-001",
                .FechaEmision = DateTime.Today.AddDays(-5).ToString(FormatoFecha),
                .HoraEmision = DateTime.Now.ToString("HH:mm:ss"),
                .Moneda = "PEN",
                .TipoDocumento = "01",
                .TipoOperacion = "1001",
                .CuentaBancoNacion = "00047-345",
                .MontoDetraccion = 99.12D,
                .TasaDetraccion = 4,
                .CodigoBienOServicio = "027",
                .CodigoMedioPago = "001",
                .TotalIgv = 18,
                .TotalVenta = 118,
                .Gravadas = 100,
                .Items = New List(Of DetalleDocumento)() From {
                    New DetalleDocumento() With {
                    .Id = 1,
                    .Cantidad = 5,
                    .PrecioReferencial = 20,
                    .PrecioUnitario = 20,
                    .TipoPrecio = "01",
                    .CodigoItem = "1234234",
                    .Descripcion = "Transporte",
                    .UnidadMedida = "KG",
                    .Impuesto = 18,
                    .TipoImpuesto = "10",
                    .TotalVenta = 100,
                    .UbigeoOrigen = "150101",
                    .DireccionOrigen = "Av. Argentina 2388",
                    .UbigeoDestino = "160101",
                    .DireccionDestino = "Jr. Morona 171",
                    .DetalleViaje = "Viaje con carga pesada",
                    .ValorReferencial = 500,
                    .ValorReferencialCargaEfectiva = 520,
                    .ValorReferencialCargaUtil = 480
                    }
                }
            }

            FirmaryEnviar(documento, GenerarDocumento(documento))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try
    End Sub

    Private Shared Sub CrearFacturaConAnticipo()
        Try
            Console.WriteLine("Ejemplo Factura de Anticipo Y Regularización del mismo (FF60-1500 y FF60-1501)")
            ' Gravada
            ' (PrecioUnitario * Cantidad) + IGV
            Dim documento = New DocumentoElectronico() With {
                .Emisor = CrearEmisor(),
                .Receptor = New Compania() With {
                    .NroDocumento = "20565211600",
                    .TipoDocumento = "6",
                    .NombreLegal = "WASPE PERU S.A.C."
                },
                .IdDocumento = "FF60-1500",
                .FechaEmision = DateTime.Today.ToString(FormatoFecha),
                .HoraEmision = DateTime.Now.ToString("HH:mm:ss"),
                .FechaVencimiento = DateTime.Today.AddDays(7).ToString(FormatoFecha),
                .Moneda = "PEN",
                .TipoDocumento = "01",
                .TipoOperacion = "0101",
                .TotalIgv = 126,
                .TotalVenta = 826,
                .Gravadas = 700,
                .Items = New List(Of DetalleDocumento)() From {
                    New DetalleDocumento() With {
                    .Id = 1,
                    .Cantidad = 1,
                    .PrecioReferencial = 700,
                    .PrecioUnitario = 700,
                    .TipoPrecio = "01",
                    .CodigoItem = "DES-02",
                    .Descripcion = "OPENINVOICEPERU UBL 2.1 ANTICIPO 50%",
                    .UnidadMedida = "NIU",
                    .Impuesto = 126,
                    .TipoImpuesto = "10",
                    .TotalVenta = 826
                    }
                }
            }

            'Especificamos el Documento Previo
            'Moneda del Documento Anterior
            'Monto Pagado previamente
            ' Tipo de Documento del Anticipo (Catalogo 12),
            ' Gravada
            ' (PrecioUnitario * Cantidad) + IGV
            Dim documentoRegularizador = New DocumentoElectronico() With {
                .Emisor = CrearEmisor(),
                .Receptor = New Compania() With {
                    .NroDocumento = "20565211600",
                    .TipoDocumento = "6",
                    .NombreLegal = "WASPE PERU S.A.C."
                },
                .IdDocumento = "FF60-1501",
                .FechaEmision = DateTime.Today.ToString(FormatoFecha),
                .HoraEmision = DateTime.Now.ToString("HH:mm:ss"),
                .Moneda = "PEN",
                .TipoDocumento = "01",
                .TipoOperacion = "0101",
                .DocAnticipo = "FF60-1500",
                .MonedaAnticipo = "PEN",
                .MontoAnticipo = 826,
                .TipoDocAnticipo = "02",
                .TotalIgv = 126,
                .TotalVenta = 826,
                .Gravadas = 700,
                .Items = New List(Of DetalleDocumento)() From {
                    New DetalleDocumento() With {
                        .Id = 1,
                        .Cantidad = 1,
                        .PrecioReferencial = 700,
                        .PrecioUnitario = 700,
                        .TipoPrecio = "01",
                        .CodigoItem = "DES-02",
                        .Descripcion = "OPENINVOICEPERU UBL 2.1 REGULARIZACIÓN 50%",
                        .UnidadMedida = "NIU",
                        .Impuesto = 126,
                        .TipoImpuesto = "10",
                        .TotalVenta = 826
                    }
                }
            }

            FirmaryEnviar(documento, GenerarDocumento(documento))

            FirmaryEnviar(documentoRegularizador, GenerarDocumento(documentoRegularizador))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try
    End Sub

    Private Shared Sub CrearFacturaGratuita()
        Try
            Console.WriteLine("Ejemplo Factura Gratuita (FF20-005)")
            'DateTime.Now.ToString("HH:mm:ss"),
            ' Gratuita
            Dim documento = New DocumentoElectronico() With {
                .Emisor = CrearEmisor(),
                .Receptor = New Compania() With {
                    .NroDocumento = "20100039207",
                    .TipoDocumento = "6",
                    .NombreLegal = "RANSA COMERCIAL S.A."
                },
                .IdDocumento = "FF20-005",
                .FechaEmision = DateTime.Today.ToString(FormatoFecha),
                .HoraEmision = "12:00:00",
                .Moneda = "PEN",
                .TipoDocumento = "01",
                .TotalIgv = 0,
                .TotalVenta = 0,
                .Gratuitas = 100,
                .Items = New List(Of DetalleDocumento)() From {
                    New DetalleDocumento() With {
                    .Id = 1,
                    .Cantidad = 5,
                    .PrecioReferencial = 20,
                    .PrecioUnitario = 0,
                    .TipoPrecio = "01",
                    .CodigoItem = "1234234",
                    .Descripcion = "Arroz Costeño",
                    .UnidadMedida = "KG",
                    .Impuesto = 0,
                    .TipoImpuesto = "21",
                    .TotalVenta = 0
                    }
                }
            }

            FirmaryEnviar(documento, GenerarDocumento(documento))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try
    End Sub

    Private Shared Sub CrearBoleta()
        Try
            Console.WriteLine("Ejemplo Boleta (BB11-001)")
            ' Gravada
            Dim documento = New DocumentoElectronico() With {
                .Emisor = CrearEmisor(),
                .Receptor = New Compania() With {
                    .NroDocumento = "88888888",
                    .TipoDocumento = "1",
                    .NombreLegal = "CLIENTE GENERICO"
                },
                .IdDocumento = "BB11-001",
                .FechaEmision = DateTime.Today.AddDays(-5).ToString(FormatoFecha),
                .HoraEmision = DateTime.Now.ToString("HH:mm:ss"),
                .Moneda = "PEN",
                .TipoDocumento = "03",
                .TotalIgv = 18,
                .TotalVenta = 118,
                .Gravadas = 100,
                .Items = New List(Of DetalleDocumento)() From {
                    New DetalleDocumento() With {
                    .Id = 1,
                    .Cantidad = 10,
                    .PrecioReferencial = 10,
                    .PrecioUnitario = 10,
                    .TipoPrecio = "01",
                    .CodigoItem = "2435675",
                    .Descripcion = "USB Kingston ©",
                    .UnidadMedida = "NIU",
                    .Impuesto = 18,
                    .TipoImpuesto = "10",
                    .TotalVenta = 100
                    }
                }
            }

            FirmaryEnviar(documento, GenerarDocumento(documento))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try
    End Sub

    Private Shared Sub CrearNotaCredito()
        Try
            Console.WriteLine("Ejemplo Nota de Crédito de Factura (FN11-001)")
            ' Gravada
            Dim documento = New DocumentoElectronico() With {
                .Emisor = CrearEmisor(),
                .Receptor = New Compania() With {
                    .NroDocumento = "20549202960",
                    .TipoDocumento = "6",
                    .NombreLegal = "EMPRESA DE SOFT",
                    .CodigoAnexo = ""
                },
                .IdDocumento = "FN11-001",
                .FechaEmision = DateTime.Today.AddDays(-5).ToString(FormatoFecha),
                .HoraEmision = DateTime.Now.ToString("HH:mm:ss"),
                .MontoEnLetras = String.Empty,
                .Moneda = "PEN",
                .TipoDocumento = "07",
                .TotalIgv = 3.97D,
                .TotalVenta = 26.04D,
                .Gravadas = 22.07D,
                .Items = New List(Of DetalleDocumento)() From {
                    New DetalleDocumento() With {
                    .Id = 1,
                    .Cantidad = 1,
                    .PrecioReferencial = 22.07D,
                    .PrecioUnitario = 22.07D,
                    .TipoPrecio = "01",
                    .CodigoItem = "2435675",
                    .Descripcion = "Corrección Factura",
                    .UnidadMedida = "NIU",
                    .Impuesto = 3.97D,
                    .TipoImpuesto = "10",
                    .TotalVenta = 22.07D
                    }
                },
                .Discrepancias = New List(Of Discrepancia)() From {
                    New Discrepancia() With {
                    .NroReferencia = "FF11-001",
                    .Tipo = "01",
                    .Descripcion = "Anulacion de la operacion"
                    }
                },
                .Relacionados = New List(Of DocumentoRelacionado)() From {
                    New DocumentoRelacionado() With {
                    .NroDocumento = "FF11-001",
                    .TipoDocumento = "01"
                    }
                }
            }

            File.WriteAllText("notacredito.json", Newtonsoft.Json.JsonConvert.SerializeObject(documento))

            FirmaryEnviar(documento, GenerarDocumento(documento))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try
    End Sub

    Private Shared Sub CrearNotaDebito()
        Try
            Console.WriteLine("Ejemplo Nota de Débito de Factura (FD11-001)")
            ' Gravada
            Dim documento = New DocumentoElectronico() With {
                .Emisor = CrearEmisor(),
                .Receptor = New Compania() With {
                    .NroDocumento = "20257471609",
                    .TipoDocumento = "6",
                    .NombreLegal = "FRAMEWORK PERU"
                },
                .IdDocumento = "FD11-001",
                .FechaEmision = DateTime.Today.ToString(FormatoFecha),
                .HoraEmision = DateTime.Now.ToString("HH:mm:ss"),
                .Moneda = "PEN",
                .TipoDocumento = "08",
                .TotalIgv = 0.76D,
                .TotalVenta = 5,
                .Gravadas = 4.24D,
                .Items = New List(Of DetalleDocumento)() From {
                    New DetalleDocumento() With {
                    .Id = 1,
                    .Cantidad = 1,
                    .PrecioReferencial = 4.24D,
                    .PrecioUnitario = 4.24D,
                    .TipoPrecio = "01",
                    .CodigoItem = "2435675",
                    .Descripcion = "Penalidad por atraso de pago",
                    .UnidadMedida = "NIU",
                    .Impuesto = 0.76D,
                    .TipoImpuesto = "10",
                    .TotalVenta = 5
                    }
                },
                .Discrepancias = New List(Of Discrepancia)() From {
                    New Discrepancia() With {
                    .NroReferencia = "FF11-001",
                    .Tipo = "03",
                    .Descripcion = "Penalidad por falta de pago"
                    }
                },
                .Relacionados = New List(Of DocumentoRelacionado)() From {
                    New DocumentoRelacionado() With {
                    .NroDocumento = "FF11-001",
                    .TipoDocumento = "01"
                    }
                }
            }

            FirmaryEnviar(documento, GenerarDocumento(documento))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try
    End Sub

    Private Shared Sub CrearResumenDiario()
        Try
            Console.WriteLine("Ejemplo de Resumen Diario")
            Dim documentoResumenDiario = New ResumenDiarioNuevo() With {
                .IdDocumento = $"RC-{DateTime.Today:yyyyMMdd}-001",
                .FechaEmision = DateTime.Today.ToString(FormatoFecha),
                .FechaReferencia = DateTime.Today.AddDays(-1).ToString(FormatoFecha),
                .Emisor = CrearEmisor(),
                .Resumenes = New List(Of GrupoResumenNuevo)()
            }

            ' 1 - Agregar. 2 - Modificar. 3 - Eliminar
            documentoResumenDiario.Resumenes.Add(New GrupoResumenNuevo() With {
                .Id = 1,
                .TipoDocumento = "03",
                .IdDocumento = "BB14-33386",
                .NroDocumentoReceptor = "41614074",
                .TipoDocumentoReceptor = "1",
                .CodigoEstadoItem = 1,
                .Moneda = "PEN",
                .TotalVenta = 190.9D,
                .TotalIgv = 29.12D,
                .Gravadas = 161.78D
            })
            ' Para los casos de envio de boletas anuladas, se debe primero informar las boletas creadas (1) y luego en un segundo resumen se envian las anuladas. De lo contrario se presentará el error 'El documento indicado no existe no puede ser modificado/eliminado'
            ' 1 - Agregar. 2 - Modificar. 3 - Eliminar
            documentoResumenDiario.Resumenes.Add(New GrupoResumenNuevo() With {
                .Id = 2,
                .TipoDocumento = "03",
                .IdDocumento = "BB30-33384",
                .NroDocumentoReceptor = "08506678",
                .TipoDocumentoReceptor = "1",
                .CodigoEstadoItem = 1,
                .Moneda = "USD",
                .TotalVenta = 9580D,
                .TotalIgv = 1411.36D,
                .Gravadas = 8168.64D
            })


            Console.WriteLine("Generando XML....")

            Dim documentoResponse = RestHelper(Of ResumenDiarioNuevo, DocumentoResponse).Execute("GenerarResumenDiario/v2", documentoResumenDiario)

            If Not documentoResponse.Exito Then
                Throw New InvalidOperationException(documentoResponse.MensajeError)
            End If

            Console.WriteLine("Firmando XML...")
            ' Firmado del Documento.
            Dim firmado = New FirmadoRequest() With {
                .TramaXmlSinFirma = documentoResponse.TramaXmlSinFirma,
                .CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes("Certificado.pfx")),
                .PasswordCertificado = String.Empty
            }

            Dim responseFirma = RestHelper(Of FirmadoRequest, FirmadoResponse).Execute("Firmar", firmado)

            If Not responseFirma.Exito Then
                Throw New InvalidOperationException(responseFirma.MensajeError)
            End If

            Console.WriteLine("Guardando XML de Resumen....(Revisar carpeta del ejecutable)")

            File.WriteAllBytes("resumendiario.xml", Convert.FromBase64String(responseFirma.TramaXmlFirmado))

            Console.WriteLine("Enviando a SUNAT....")

            Dim enviarDocumentoRequest = New EnviarDocumentoRequest() With {
                .Ruc = documentoResumenDiario.Emisor.NroDocumento,
                .UsuarioSol = "MODDATOS",
                .ClaveSol = "MODDATOS",
                .EndPointUrl = UrlSunat,
                .IdDocumento = documentoResumenDiario.IdDocumento,
                .TramaXmlFirmado = responseFirma.TramaXmlFirmado
            }

            Dim enviarResumenResponse = RestHelper(Of EnviarDocumentoRequest, EnviarResumenResponse).Execute("EnviarResumen", enviarDocumentoRequest)

            If Not enviarResumenResponse.Exito Then
                Throw New InvalidOperationException(enviarResumenResponse.MensajeError)
            End If

            Console.WriteLine("Nro de Ticket: {0}", enviarResumenResponse.NroTicket)

            ConsultarTicket(enviarResumenResponse.NroTicket, documentoResumenDiario.Emisor.NroDocumento)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try
    End Sub

    Private Shared Sub CrearComunicacionBaja()
        Try
            Console.WriteLine("Ejemplo de Comunicación de Baja")
            Dim documentoBaja = New ComunicacionBaja() With {
                .IdDocumento = $"RA-{DateTime.Today:yyyyMMdd}-001",
                .FechaEmision = DateTime.Today.ToString(FormatoFecha),
                .FechaReferencia = DateTime.Today.AddDays(-1).ToString(FormatoFecha),
                .Emisor = CrearEmisor(),
                .Bajas = New List(Of DocumentoBaja)()
            }

            ' En las comunicaciones de Baja ya no se pueden colocar boletas, ya que la anulacion de las mismas
            ' la realiza el resumen diario.
            documentoBaja.Bajas.Add(New DocumentoBaja() With {
                .Id = 1,
                .Correlativo = "33386",
                .TipoDocumento = "01",
                .Serie = "FA50",
                .MotivoBaja = "Anulación por otro tipo de documento"
            })
            documentoBaja.Bajas.Add(New DocumentoBaja() With {
                .Id = 2,
                .Correlativo = "86486",
                .TipoDocumento = "01",
                .Serie = "FF14",
                .MotivoBaja = "Anulación por otro datos erroneos"
            })

            Console.WriteLine("Generando XML....")

            Dim documentoResponse = RestHelper(Of ComunicacionBaja, DocumentoResponse).Execute("GenerarComunicacionBaja", documentoBaja)
            If Not documentoResponse.Exito Then
                Throw New InvalidOperationException(documentoResponse.MensajeError)
            End If

            Console.WriteLine("Firmando XML...")
            ' Firmado del Documento.
            Dim firmado = New FirmadoRequest() With {
                .TramaXmlSinFirma = documentoResponse.TramaXmlSinFirma,
                .CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes("Certificado.pfx")),
                .PasswordCertificado = String.Empty
            }

            Dim responseFirma = RestHelper(Of FirmadoRequest, FirmadoResponse).Execute("Firmar", firmado)

            If Not responseFirma.Exito Then
                Throw New InvalidOperationException(responseFirma.MensajeError)
            End If

            Console.WriteLine("Guardando XML de la Comunicacion de Baja....(Revisar carpeta del ejecutable)")

            File.WriteAllBytes("comunicacionbaja.xml", Convert.FromBase64String(responseFirma.TramaXmlFirmado))

            Console.WriteLine("Enviando a SUNAT....")

            Dim sendBill = New EnviarDocumentoRequest() With {
                .Ruc = documentoBaja.Emisor.NroDocumento,
                .UsuarioSol = "MODDATOS",
                .ClaveSol = "MODDATOS",
                .EndPointUrl = UrlSunat,
                .IdDocumento = documentoBaja.IdDocumento,
                .TramaXmlFirmado = responseFirma.TramaXmlFirmado
            }

            Dim enviarResumenResponse = RestHelper(Of EnviarDocumentoRequest, EnviarResumenResponse).Execute("EnviarResumen", sendBill)

            If Not enviarResumenResponse.Exito Then
                Throw New InvalidOperationException(enviarResumenResponse.MensajeError)
            End If

            Console.WriteLine("Nro de Ticket: {0}", enviarResumenResponse.NroTicket)

            ConsultarTicket(enviarResumenResponse.NroTicket, documentoBaja.Emisor.NroDocumento)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try
    End Sub

    Private Shared Sub ConsultarTicket(nroTicket As String, nroRuc As String)
        Dim consultarTicketRequest = New ConsultaTicketRequest() With {
            .Ruc = nroRuc,
            .NroTicket = nroTicket,
            .UsuarioSol = "MODDATOS",
            .ClaveSol = "MODDATOS",
            .EndPointUrl = UrlSunat
        }

        Dim response = RestHelper(Of ConsultaTicketRequest, EnviarDocumentoResponse).Execute("ConsultarTicket", consultarTicketRequest)

        If Not response.Exito Then
            Console.WriteLine(response.MensajeError)
            Return
        End If

        Dim archivo = response.NombreArchivo.Replace(".xml", String.Empty)
        Console.WriteLine($"Escribiendo documento en la carpeta del ejecutable... {archivo}")

        File.WriteAllBytes($"{archivo}.zip", Convert.FromBase64String(response.TramaZipCdr))

        Console.WriteLine($"Código: {response.CodigoRespuesta} => {response.MensajeRespuesta}")
    End Sub

    Private Shared Sub CrearDocumentoRetencion()
        Try
            Console.WriteLine("Ejemplo de Retención (R001-123)")
            Dim documento = New DocumentoRetencion() With {
                .Emisor = ToNegocio(CrearEmisor()),
                .Receptor = New Negocio() With {
                    .NroDocumento = "20100039207",
                    .TipoDocumento = "6",
                    .NombreLegal = "RANSA COMERCIAL S.A.",
                    .Ubigeo = "150101",
                    .Direccion = "Av. Argentina 2833",
                    .Urbanizacion = "-",
                    .Departamento = "CALLAO",
                    .Provincia = "CALLAO",
                    .Distrito = "CALLAO"
                },
                .IdDocumento = "R001-123",
                .FechaEmision = DateTime.Today.ToString(FormatoFecha),
                .Moneda = "PEN",
                .RegimenRetencion = "01",
                .TasaRetencion = 3,
                .ImporteTotalRetenido = 300,
                .ImporteTotalPagado = 10000,
                .Observaciones = "Emision de Facturas del periodo Dic. 2016",
                .DocumentosRelacionados = New List(Of ItemRetencion)() From {
                    New ItemRetencion() With {
                    .NroDocumento = "E001-457",
                    .TipoDocumento = "01",
                    .MonedaDocumentoRelacionado = "USD",
                    .FechaEmision = DateTime.Today.AddDays(-3).ToString(FormatoFecha),
                    .ImporteTotal = 10000,
                    .FechaPago = DateTime.Today.ToString(FormatoFecha),
                    .NumeroPago = 153,
                    .ImporteSinRetencion = 9700,
                    .ImporteRetenido = 300,
                    .FechaRetencion = DateTime.Today.ToString(FormatoFecha),
                    .ImporteTotalNeto = 10000,
                    .TipoCambio = 3.41D,
                    .FechaTipoCambio = DateTime.Today.ToString(FormatoFecha)
                    }
                    }
            }

            Console.WriteLine("Generando XML....")

            Dim documentoResponse = RestHelper(Of DocumentoRetencion, DocumentoResponse).Execute("GenerarRetencion", documento)

            If Not documentoResponse.Exito Then
                Throw New InvalidOperationException(documentoResponse.MensajeError)
            End If

            Console.WriteLine("Firmando XML...")
            ' Firmado del Documento.
            Dim firmado = New FirmadoRequest() With {
                .TramaXmlSinFirma = documentoResponse.TramaXmlSinFirma,
                .CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes("certificado.pfx")),
                .PasswordCertificado = String.Empty
            }

            Dim responseFirma = RestHelper(Of FirmadoRequest, FirmadoResponse).Execute("Firmar", firmado)

            If Not responseFirma.Exito Then
                Throw New InvalidOperationException(responseFirma.MensajeError)
            End If

            File.WriteAllBytes("retencion.xml", Convert.FromBase64String(responseFirma.TramaXmlFirmado))

            Console.WriteLine("Enviando Retención a SUNAT....")

            Dim enviarDocumentoRequest = New EnviarDocumentoRequest() With {
                .Ruc = documento.Emisor.NroDocumento,
                .UsuarioSol = "MODDATOS",
                .ClaveSol = "MODDATOS",
                .EndPointUrl = UrlOtroCpe,
                .IdDocumento = documento.IdDocumento,
                .TipoDocumento = "20",
                .TramaXmlFirmado = responseFirma.TramaXmlFirmado
            }

            Dim enviarDocumentoResponse = RestHelper(Of EnviarDocumentoRequest, EnviarDocumentoResponse).Execute("EnviarDocumento", enviarDocumentoRequest)

            If Not enviarDocumentoResponse.Exito Then
                Throw New InvalidOperationException(enviarDocumentoResponse.MensajeError)
            End If

            Console.WriteLine("Respuesta de SUNAT:")
            Console.WriteLine(enviarDocumentoResponse.MensajeRespuesta)

            File.WriteAllBytes("retencioncdr.zip", Convert.FromBase64String(enviarDocumentoResponse.TramaZipCdr))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try
    End Sub

    Private Shared Sub CrearDocumentoPercepcion()
        Try
            Console.WriteLine("Ejemplo de Percepción (P001-123)")
            Dim documento = New DocumentoPercepcion() With {
                .Emisor = ToNegocio(CrearEmisor()),
                .Receptor = New Negocio() With {
                    .NroDocumento = "20100039207",
                    .TipoDocumento = "6",
                    .NombreLegal = "RANSA COMERCIAL S.A.",
                    .Ubigeo = "150101",
                    .Direccion = "Av. Argentina 2833",
                    .Urbanizacion = "-",
                    .Departamento = "CALLAO",
                    .Provincia = "CALLAO",
                    .Distrito = "CALLAO"
                },
                .IdDocumento = "P001-123",
                .FechaEmision = DateTime.Today.ToString(FormatoFecha),
                .Moneda = "PEN",
                .RegimenPercepcion = "01",
                .TasaPercepcion = 2,
                .ImporteTotalPercibido = 200,
                .ImporteTotalCobrado = 10000,
                .Observaciones = "Emision de Facturas del periodo Dic. 2016",
                .DocumentosRelacionados = New List(Of ItemPercepcion)() From {
                    New ItemPercepcion() With {
                    .NroDocumento = "E001-457",
                    .TipoDocumento = "01",
                    .MonedaDocumentoRelacionado = "USD",
                    .FechaEmision = DateTime.Today.AddDays(-3).ToString(FormatoFecha),
                    .ImporteTotal = 10000,
                    .FechaPago = DateTime.Today.ToString(FormatoFecha),
                    .NumeroPago = 153,
                    .ImporteSinPercepcion = 9800,
                    .ImportePercibido = 200,
                    .FechaPercepcion = DateTime.Today.ToString(FormatoFecha),
                    .ImporteTotalNeto = 10000,
                    .TipoCambio = 3.41D,
                    .FechaTipoCambio = DateTime.Today.ToString(FormatoFecha)
                    }
                    }
            }

            Console.WriteLine("Generando XML....")

            Dim documentoResponse = RestHelper(Of DocumentoPercepcion, DocumentoResponse).Execute("GenerarPercepcion", documento)

            If Not documentoResponse.Exito Then
                Throw New InvalidOperationException(documentoResponse.MensajeError)
            End If

            Console.WriteLine("Firmando XML...")
            ' Firmado del Documento.
            Dim firmado = New FirmadoRequest() With {
                .TramaXmlSinFirma = documentoResponse.TramaXmlSinFirma,
                .CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes("certificado.pfx")),
                .PasswordCertificado = String.Empty
            }

            Dim responseFirma = RestHelper(Of FirmadoRequest, FirmadoResponse).Execute("Firmar", firmado)

            If Not responseFirma.Exito Then
                Throw New InvalidOperationException(responseFirma.MensajeError)
            End If

            File.WriteAllBytes("percepcion.xml", Convert.FromBase64String(responseFirma.TramaXmlFirmado))

            Console.WriteLine("Enviando Retención a SUNAT....")

            Dim sendBill = New EnviarDocumentoRequest() With {
                .Ruc = documento.Emisor.NroDocumento,
                .UsuarioSol = "MODDATOS",
                .ClaveSol = "MODDATOS",
                .EndPointUrl = UrlOtroCpe,
                .IdDocumento = documento.IdDocumento,
                .TipoDocumento = "40",
                .TramaXmlFirmado = responseFirma.TramaXmlFirmado
            }

            Dim responseSendBill = RestHelper(Of EnviarDocumentoRequest, EnviarDocumentoResponse).Execute("EnviarDocumento", sendBill)

            If Not responseSendBill.Exito Then
                Throw New InvalidOperationException(responseSendBill.MensajeError)
            End If

            Console.WriteLine("Respuesta de SUNAT:")
            Console.WriteLine(responseSendBill.MensajeRespuesta)

            File.WriteAllBytes("percepcioncdr.zip", Convert.FromBase64String(responseSendBill.TramaZipCdr))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try
    End Sub

    Private Shared Sub CrearGuiaRemision()
        Try
            Console.WriteLine("Ejemplo de Guia de Remisión (TAAA-2344)")
            Dim guia = New GuiaRemision() With {
                .IdDocumento = "TAAA-2344",
                .FechaEmision = DateTime.Today.ToString(FormatoFecha),
                .TipoDocumento = "09",
                .Glosa = "Guia de Prueba",
                .Remitente = CrearEmisor(),
                .Destinatario = New Contribuyente() With {
                    .NroDocumento = "20100039207",
                    .TipoDocumento = "6",
                    .NombreLegal = "RANSA COMERCIAL S.A."
                },
                .ShipmentId = "001",
                .CodigoMotivoTraslado = "01",
                .DescripcionMotivo = "VENTA DIRECTA",
                .Transbordo = False,
                .PesoBrutoTotal = 50,
                .NroPallets = 0,
                .ModalidadTraslado = "01",
                .FechaInicioTraslado = DateTime.Today.ToString(FormatoFecha),
                .RucTransportista = "20257471609",
                .RazonSocialTransportista = "FRAMEWORK PERU",
                .NroPlacaVehiculo = "YG-9244",
                .NroDocumentoConductor = "88888888",
                .DireccionPartida = New Direccion() With {
                    .Ubigeo = "150119",
                    .DireccionCompleta = "AV. ARAMBURU 878"
                },
                .DireccionLlegada = New Direccion() With {
                    .Ubigeo = "150101",
                    .DireccionCompleta = "AV. ARGENTINA 2388"
                },
                .NumeroContenedor = String.Empty,
                .CodigoPuerto = String.Empty,
                .BienesATransportar = New List(Of DetalleGuia)() From {
                    New DetalleGuia() With {
                    .Correlativo = 1,
                    .CodigoItem = "XXXX",
                    .Descripcion = "XXXXXXX",
                    .UnidadMedida = "NIU",
                    .Cantidad = 4,
                    .LineaReferencia = 1
                    }
                    }
            }

            Console.WriteLine("Generando XML....")

            Dim documentoResponse = RestHelper(Of GuiaRemision, DocumentoResponse).Execute("GenerarGuiaRemision", guia)

            If Not documentoResponse.Exito Then
                Throw New InvalidOperationException(documentoResponse.MensajeError)
            End If

            Console.WriteLine("Firmando XML...")
            ' Firmado del Documento.
            Dim firmado = New FirmadoRequest() With {
                .TramaXmlSinFirma = documentoResponse.TramaXmlSinFirma,
                .CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes("certificado.pfx")),
                .PasswordCertificado = String.Empty
            }

            Dim responseFirma = RestHelper(Of FirmadoRequest, FirmadoResponse).Execute("Firmar", firmado)

            If Not responseFirma.Exito Then
                Throw New InvalidOperationException(responseFirma.MensajeError)
            End If

            File.WriteAllBytes("GuiaRemision.xml", Convert.FromBase64String(responseFirma.TramaXmlFirmado))

            Console.WriteLine("Enviando a SUNAT....")

            Dim documentoRequest = New EnviarDocumentoRequest() With {
                .Ruc = guia.Remitente.NroDocumento,
                .UsuarioSol = "MODDATOS",
                .ClaveSol = "MODDATOS",
                .EndPointUrl = UrlGuiaRemision,
                .IdDocumento = guia.IdDocumento,
                .TipoDocumento = guia.TipoDocumento,
                .TramaXmlFirmado = responseFirma.TramaXmlFirmado
            }

            Dim enviarDocumentoResponse = RestHelper(Of EnviarDocumentoRequest, EnviarDocumentoResponse).Execute("EnviarDocumento", documentoRequest)

            If Not enviarDocumentoResponse.Exito Then
                Throw New InvalidOperationException(enviarDocumentoResponse.MensajeError)
            End If

            File.WriteAllBytes("GuaiRemisionCdr.zip", Convert.FromBase64String(enviarDocumentoResponse.TramaZipCdr))

            Console.WriteLine("Respuesta de SUNAT:")
            Console.WriteLine(enviarDocumentoResponse.MensajeRespuesta)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try

    End Sub

    Private Shared Function GenerarDocumento(documento As DocumentoElectronico) As DocumentoResponse
        Console.WriteLine("Generando XML....")

        Dim metodo = "GenerarFactura"
        Select Case documento.TipoDocumento
            Case "01", "03"
                metodo = "GenerarFactura"
                Exit Select
            Case "07"
                metodo = "GenerarNotaCredito"
                Exit Select
            Case "08"
                metodo = "GenerarNotaDebito"
                Exit Select
        End Select

        Dim documentoResponse = RestHelper(Of DocumentoElectronico, DocumentoResponse).Execute(metodo, documento)

        If Not documentoResponse.Exito Then
            Throw New InvalidOperationException(documentoResponse.MensajeError)
        End If

        Return documentoResponse
    End Function

    Private Shared Sub FirmaryEnviar(documento As DocumentoElectronico, documentoResponse As DocumentoResponse)
        Console.WriteLine("Firmando XML...")

        ' Firmado del Documento.
        Dim firmado = New FirmadoRequest() With {
            .TramaXmlSinFirma = documentoResponse.TramaXmlSinFirma,
            .CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes("certificado.pfx")),
            .PasswordCertificado = String.Empty
        }

        Dim responseFirma = RestHelper(Of FirmadoRequest, FirmadoResponse).Execute("Firmar", firmado)

        If Not responseFirma.Exito Then
            Throw New InvalidOperationException(responseFirma.MensajeError)
        End If

        Console.WriteLine("Escribiendo el archivo {0}.xml .....", documento.IdDocumento)

        File.WriteAllBytes($"{documento.IdDocumento}.xml", Convert.FromBase64String(responseFirma.TramaXmlFirmado))

        Process.Start($"{documento.IdDocumento}.xml")

        Console.WriteLine("Enviando a SUNAT....")

        Dim documentoRequest = New EnviarDocumentoRequest() With {
            .Ruc = documento.Emisor.NroDocumento,
            .UsuarioSol = "MODDATOS",
            .ClaveSol = "MODDATOS",
            .EndPointUrl = UrlSunat,
            .IdDocumento = documento.IdDocumento,
            .TipoDocumento = documento.TipoDocumento,
            .TramaXmlFirmado = responseFirma.TramaXmlFirmado
        }

        Dim enviarDocumentoResponse = RestHelper(Of EnviarDocumentoRequest, EnviarDocumentoResponse).Execute("EnviarDocumento", documentoRequest)

        If Not enviarDocumentoResponse.Exito Then
            Throw New InvalidOperationException(enviarDocumentoResponse.MensajeError)
        End If

        File.WriteAllBytes($"{documento.IdDocumento}.zip", Convert.FromBase64String(enviarDocumentoResponse.TramaZipCdr))

        Console.WriteLine("Respuesta de SUNAT:")
        Console.WriteLine(enviarDocumentoResponse.MensajeRespuesta)
    End Sub

    Private Shared Sub DescargarComprobante()
        Try
            Console.WriteLine("Consulta de Comprobantes Electrónicos (solo Producción)")
            Dim ruc = LeerLinea("Ingrese su Nro. de RUC")
            Dim usuario = LeerLinea("Ingrese usuario SOL")
            Dim clave = LeerLinea("Ingrese Clave SOL")
            Dim tipoDoc = LeerLinea("Ingrese Codigo Tipo de Documento a Consultar (01, 03, 07 o 08)")
            Dim serie = LeerLinea("Ingrese Serie Documento a Leer")
            Dim correlativo = LeerLinea("Ingrese el correlativo del documento sin ceros")

            Dim consultaConstanciaRequest = New ConsultaConstanciaRequest() With {
                .UsuarioSol = usuario,
                .ClaveSol = clave,
                .TipoDocumento = tipoDoc,
                .Serie = serie,
                .Numero = Convert.ToInt32(correlativo),
                .Ruc = ruc,
                .EndPointUrl = "https://e-factura.sunat.gob.pe/ol-it-wsconscpegem/billConsultService"
            }

            Dim documentoResponse = RestHelper(Of ConsultaConstanciaRequest, EnviarDocumentoResponse).Execute("ConsultarConstancia", consultaConstanciaRequest)

            If Not documentoResponse.Exito Then
                Console.WriteLine(documentoResponse.MensajeError)
                Return
            End If

            Dim archivo = documentoResponse.NombreArchivo.Replace(".xml", String.Empty)
            Console.WriteLine($"Escribiendo documento en la carpeta del ejecutable... {archivo}")

            File.WriteAllBytes($"{archivo}.zip", Convert.FromBase64String(documentoResponse.TramaZipCdr))

            Console.WriteLine($"Código: {documentoResponse.CodigoRespuesta} => {documentoResponse.MensajeRespuesta}")
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.ReadLine()
        End Try

    End Sub

    Private Shared Function LeerLinea(mensaje As String) As String
        Console.WriteLine(mensaje)
        Return Console.ReadLine()
    End Function

End Class