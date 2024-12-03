using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AjedrezWPF
{
    internal class Pieza
    {
        public bool EsBlanca { get; set; }
        public bool EsNegra { get; set; }
        public string Nombre { get; set; }

        public Pieza(bool esBlanca, bool esNegra, string nombre)
        {
            EsBlanca = esBlanca;
            EsNegra = esNegra;
            Nombre = nombre;
        }

        public List<(int fila, int columna)> GetMovimientos(int fila, int columna, Casillas[,] tablero)
        {
            List<(int fila, int columna)> resultado = new List<(int fila, int columna)>();

            switch (Nombre)
            {
                case "Peón":
                    if (EsBlanca)
                    {
                        // Movimiento hacia adelante
                        if (fila + 1 < 8 && !tablero[fila + 1, columna].HayPieza)
                        {
                            resultado.Add((fila + 1, columna));
                        }
                        // Captura en diagonal izquierda
                        if (fila + 1 < 8 && columna - 1 >= 0 && tablero[fila + 1, columna - 1].HayPieza && tablero[fila + 1, columna - 1].Pieza.EsNegra)
                        {
                            resultado.Add((fila + 1, columna - 1));
                        }
                        // Captura en diagonal derecha
                        if (fila + 1 < 8 && columna + 1 < 8 && tablero[fila + 1, columna + 1].HayPieza && tablero[fila + 1, columna + 1].Pieza.EsNegra)
                        {
                            resultado.Add((fila + 1, columna + 1));
                        }
                    }
                    else if (EsNegra)
                    {
                        // Movimiento hacia adelante
                        if (fila - 1 >= 0 && !tablero[fila - 1, columna].HayPieza)
                        {
                            resultado.Add((fila - 1, columna));
                        }
                        // Captura en diagonal izquierda
                        if (fila - 1 >= 0 && columna - 1 >= 0 && tablero[fila - 1, columna - 1].HayPieza && tablero[fila - 1, columna - 1].Pieza.EsBlanca)
                        {
                            resultado.Add((fila - 1, columna - 1));
                        }
                        // Captura en diagonal derecha
                        if (fila - 1 >= 0 && columna + 1 < 8 && tablero[fila - 1, columna + 1].HayPieza && tablero[fila - 1, columna + 1].Pieza.EsBlanca)
                        {
                            resultado.Add((fila - 1, columna + 1));
                        }
                    }
                    break;
                case "Torre":
                    AgregarMovimientosLineales(resultado, fila, columna, tablero, new (int, int)[] { (1, 0), (-1, 0), (0, 1), (0, -1) });
                    break;
                case "Caballo":
                    AgregarMovimientosCaballo(resultado, fila, columna, tablero);
                    break;
                case "Alfil":
                    AgregarMovimientosLineales(resultado, fila, columna, tablero, new (int, int)[] { (1, 1), (-1, -1), (1, -1), (-1, 1) });
                    break;
                case "Reina":
                    AgregarMovimientosLineales(resultado, fila, columna, tablero, new (int, int)[] { (1, 0), (-1, 0), (0, 1), (0, -1), (1, 1), (-1, -1), (1, -1), (-1, 1) });
                    break;
                case "Rey":
                    AgregarMovimientosRey(resultado, fila, columna, tablero);
                    break;
            }

            return resultado;
        }

        private void AgregarMovimientosLineales(List<(int fila, int columna)> resultado, int fila, int columna, Casillas[,] tablero, (int, int)[] direcciones)
        {
            foreach (var (df, dc) in direcciones)
            {
                int f = fila + df;
                int c = columna + dc;
                while (f >= 0 && f < 8 && c >= 0 && c < 8)
                {
                    if (tablero[f, c].HayPieza)
                    {
                        if (tablero[f, c].Pieza.EsBlanca != EsBlanca)
                        {
                            resultado.Add((f, c)); // Puede capturar la pieza
                        }
                        break; // Detenerse al encontrar una pieza
                    }
                    resultado.Add((f, c));
                    f += df;
                    c += dc;
                }
            }
        }

        private void AgregarMovimientosCaballo(List<(int fila, int columna)> resultado, int fila, int columna, Casillas[,] tablero)
        {
            (int, int)[] movimientos = new (int, int)[]
            {
                (2, 1), (2, -1), (-2, 1), (-2, -1),
                (1, 2), (1, -2), (-1, 2), (-1, -2)
            };

            foreach (var (df, dc) in movimientos)
            {
                int f = fila + df;
                int c = columna + dc;
                if (f >= 0 && f < 8 && c >= 0 && c < 8)
                {
                    if (!tablero[f, c].HayPieza || tablero[f, c].Pieza.EsBlanca != EsBlanca)
                    {
                        resultado.Add((f, c));
                    }
                }
            }
        }

        private void AgregarMovimientosRey(List<(int fila, int columna)> resultado, int fila, int columna, Casillas[,] tablero)
        {
            (int, int)[] movimientos = new (int, int)[]
            {
                (1, 0), (-1, 0), (0, 1), (0, -1),
                (1, 1), (-1, -1), (1, -1), (-1, 1)
            };

            foreach (var (df, dc) in movimientos)
            {
                int f = fila + df;
                int c = columna + dc;
                if (f >= 0 && f < 8 && c >= 0 && c < 8)
                {
                    if (!tablero[f, c].HayPieza || tablero[f, c].Pieza.EsBlanca != EsBlanca)
                    {
                        resultado.Add((f, c));
                    }
                }
            }
        }
    }
}