using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AjedrezWPF
{
    internal class Casillas : Button
    {
        public bool HayPieza { get; set; }
        public Pieza? Pieza { get; set; }
        public bool EsBlanca { get; set; }
        public bool EsNegra { get; set; }
        public Grid Tablero { get; set; }
        public String ColorFondo { get; set; }
        private static Casillas? casillaSeleccionada;
        private static int? Turno = 1;
        private static Label? Turno_labels;
        private static Label? peonesBlancasLabel;
        private static Label? torresBlancasLabel;
        private static Label? alfilsBlancasLabel;
        private static Label? caballosBlancasLabel;
        private static Label? peonesNegrasLabel;
        private static Label? torresNegrasLabel;
        private static Label? alfilsNegrasLabel;
        private static Label? caballosNegrasLabel;
        private MainWindow mainWindow;


        public Casillas(int fila, int columna, Grid tablero, Label turno_labels, Label PeonesBlancasLabel, Label TorresBlancasLabel, Label AlfilsBlancasLabel, Label CaballosBlancasLabel, Label PeonesNegrasLabel, Label TooresNegrasLabel, Label AlfilsNegrasLabel, Label CaballosNegrasLabel) : base()
        {
            EsBlanca = (fila + columna) % 2 == 0;
            EsNegra = !EsBlanca;
            PintarCasilla();
            Tablero = tablero;
            ColorFondo = Background.ToString();
            Turno_labels = turno_labels;
            peonesBlancasLabel = PeonesBlancasLabel;
            torresBlancasLabel = TorresBlancasLabel;
            alfilsBlancasLabel = AlfilsBlancasLabel;
            caballosBlancasLabel = CaballosBlancasLabel;
            peonesNegrasLabel = PeonesNegrasLabel;
            torresNegrasLabel = TooresNegrasLabel;
            alfilsNegrasLabel = AlfilsNegrasLabel;
            caballosNegrasLabel = CaballosNegrasLabel;



        }

        public void PintarCasilla()
        {
            if (EsBlanca)
            {
                Background = Brushes.LightGray;
            }
            else
            {
                Background = Brushes.Gray;
            }
        }



        public void DefinirDimensiones(int ancho, int alto)
        {
            Width = ancho;
            Height = alto;
        }

        public void AgregarPieza(Pieza pieza)
        {
            string rutaImagen = $"Imgs/{pieza.Nombre}_{pieza.GetColor()}.png";
            Pieza = pieza;
            Image imagenPieza = new Image
            {
                Source = new BitmapImage(new Uri(rutaImagen, UriKind.Relative)),
                Width = 50,
                Height = 50
            };
            HayPieza = true;
            Content = imagenPieza;
            if (!EsBlanca)
            {
                Foreground = Brushes.White;
            }
        }

        public (int fila, int columna) GetPosicionCasillaEnTablero()
        {
            int fila = Grid.GetRow(this);
            int columna = Grid.GetColumn(this);
            return (fila, columna);
        }

        public void Casilla_Click(object sender, RoutedEventArgs e)
        {

            if (EstaEnJaqueMate())
            {
                MessageBox.Show("¡Jaque mate! El juego ha terminado.", "Fin del juego", MessageBoxButton.OK, MessageBoxImage.Information);
                LimpiarTablero();
                return;
                // Aquí puedes agregar lógica adicional para reiniciar el juego o finalizar la aplicación
            }
            if (casillaSeleccionada == null)
            {
                
                if (HayPieza)
                {
                    casillaSeleccionada = this;
                    if (casillaSeleccionada.Pieza.GetColor().Equals("Blanca") && Turno % 2 != 0)
                    { }
                    else if (casillaSeleccionada.Pieza.GetColor().Equals("Negra") && Turno % 2 == 0)
                    { }
                    else
                    {
                        MessageBox.Show("No puedes mover esta pieza");
                        return;
                    }
                    var posicion = GetPosicionCasillaEnTablero();
                    Casillas[,] tableroArray = new Casillas[8, 8];

                    foreach (var child in Tablero.Children)
                    {
                        if (child is Casillas casilla)
                        {
                            var (fila, columna) = casilla.GetPosicionCasillaEnTablero();
                            tableroArray[fila, columna] = casilla;
                        }
                    }

                    List<(int fila, int columna)> movimientos = Pieza.GetMovimientos(posicion.fila, posicion.columna, tableroArray);

                    // Pintar las casillas en naranja de los posibles movimientos
                    foreach (var movimiento in movimientos)
                    {
                        if (movimiento.fila >= 0 && movimiento.fila < 8 && movimiento.columna >= 0 && movimiento.columna < 8)
                        {
                            var casilla = tableroArray[movimiento.fila, movimiento.columna];
                            if (casilla != null && (casilla.Pieza == null || casilla.Pieza.EsBlanca != Pieza.EsBlanca))
                            {
                                casilla.Background = Brushes.Orange;
                            }
                        }
                    }
                }
            }
            else
            {
                if (Background == Brushes.Orange)
                {
                    // Mover la pieza a la nueva casilla
                    if (HayPieza)
                    {
                        MessageBox.Show("Has comido: " + Pieza.Nombre);
                        ModicarLabelsPieza();
                    }
                    AgregarPieza(casillaSeleccionada.Pieza);
                    Turno++;
                    if (Turno_labels != null)
                    {
                        Turno_labels.Content = Turno % 2 == 0 ? "Turno: Negras" : "Turno: Blancas";
                    }


                    casillaSeleccionada.EliminarPieza();
                    casillaSeleccionada = null;
                    // Limpiar los fondos de las casillas
                    foreach (var child in Tablero.Children)
                    {
                        if (child is Casillas casilla)
                        {
                            casilla.PintarCasilla();
                        }
                    }
                }
                else
                {
                    // Limpiar los fondos de las casillas
                    foreach (var child in Tablero.Children)
                    {
                        if (child is Casillas casilla)
                        {
                            casilla.PintarCasilla();
                        }
                    }
                    casillaSeleccionada = null;
                }
            }


        }

        //Vamos a definir el método para modifcar los contadores de los labels de las piezas comidas para cada color 
        public void ModicarLabelsPieza()
        {
            if (Pieza.EsBlanca)
            {
                switch (Pieza.Nombre)
                {
                    case "Peón":
                        peonesBlancasLabel.Content = (Convert.ToInt32(peonesBlancasLabel.Content) + 1).ToString();
                        break;
                    case "Torre":
                        torresBlancasLabel.Content = (Convert.ToInt32(torresBlancasLabel.Content) + 1).ToString();
                        break;
                    case "Alfil":
                        alfilsBlancasLabel.Content = (Convert.ToInt32(alfilsBlancasLabel.Content) + 1).ToString();
                        break;
                    case "Caballo":
                        caballosBlancasLabel.Content = (Convert.ToInt32(caballosBlancasLabel.Content) + 1).ToString();
                        break;
                }
            }
            else if (Pieza.EsNegra)
            {
                switch (Pieza.Nombre)
                {
                    case "Peón":
                        peonesNegrasLabel.Content = (Convert.ToInt32(peonesNegrasLabel.Content) + 1).ToString();
                        break;
                    case "Torre":
                        torresNegrasLabel.Content = (Convert.ToInt32(torresNegrasLabel.Content) + 1).ToString();
                        break;
                    case "Alfil":
                        alfilsNegrasLabel.Content = (Convert.ToInt32(alfilsNegrasLabel.Content) + 1).ToString();
                        break;
                    case "Caballo":
                        caballosNegrasLabel.Content = (Convert.ToInt32(caballosNegrasLabel.Content) + 1).ToString();
                        break;
                }
            }
        }

        public void EliminarPieza()
        {
            Pieza = null;
            HayPieza = false;
            Content = "";
        }


        //Meétodo para limpiar todo el tablero y reiniciar el juego 
        public void LimpiarTablero()
        {
            foreach (var child in Tablero.Children)
            {
                if (child is Casillas casilla)
                {
                    casilla.EliminarPieza();
                }
            }
            Turno = 1;
            Turno_labels.Content = "Turno: Blancas";
            peonesBlancasLabel.Content = "0";
            torresBlancasLabel.Content = "0";
            alfilsBlancasLabel.Content = "0";
            caballosBlancasLabel.Content = "0";
            peonesNegrasLabel.Content = "0";
            torresNegrasLabel.Content = "0";
            alfilsNegrasLabel.Content = "0";
            caballosNegrasLabel.Content = "0";

            mainWindow.GenerarTablero();
        }


        //Vamos a empezar a implementar los métodos para saber si es jaque mate o jaque solo

        //Primero definimos un método el cual nos va a devolver la posicion del rey de un color en el tablero
        public Casillas GetPosicionRey(string color)
        {
            Casillas[,] tableroArray = new Casillas[8, 8];
            foreach (var child in Tablero.Children)
            {
                if (child is Casillas casilla)
                {
                    var (fila, columna) = casilla.GetPosicionCasillaEnTablero();
                    tableroArray[fila, columna] = casilla;
                }
            }
            for (int i = 0; i < tableroArray.GetLength(0); i++)
            {
                for (int j = 0; j < tableroArray.GetLength(1); j++)
                {
                    if (tableroArray[i, j].HayPieza && tableroArray[i, j].Pieza.Nombre == "Rey" && tableroArray[i, j].Pieza.GetColor() == color)
                    {
                        return tableroArray[i, j];
                    }
                }
            }
            return null;
        }

        //Tendremos que comprobar en una función si el rey de un color está en jaque
        public bool EstaEnJaque(Casillas rey)
        {
            var posicionRey = rey.GetPosicionCasillaEnTablero();
            foreach (var child in Tablero.Children)
            {
                if (child is Casillas casilla)
                {
                    var (fila, columna) = casilla.GetPosicionCasillaEnTablero();
                    if (casilla.HayPieza && casilla.Pieza.GetColor() != rey.Pieza.GetColor())
                    {
                        List<(int fila, int columna)> movimientos = casilla.Pieza.GetMovimientos(fila, columna, ObtenerTablero());
                        foreach (var movimiento in movimientos)
                        {
                            if (movimiento.fila == posicionRey.fila && movimiento.columna == posicionRey.columna)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public Casillas[,] ObtenerTablero()
        {
            Casillas[,] tableroArray = new Casillas[8, 8];
            foreach (var child in Tablero.Children)
            {
                if (child is Casillas casilla)
                {
                    var (fila, columna) = casilla.GetPosicionCasillaEnTablero();
                    tableroArray[fila, columna] = casilla;
                }
            }
            return tableroArray;
        }



        private bool PuedeMoverYNoQuedarEnJaque(Casillas origen, (int fila, int columna) destino)
        {
            // Obtener el estado actual del tablero
            var tableroArray = ObtenerTablero();

            // Guardar la pieza original en la casilla de destino
            var piezaOriginal = tableroArray[destino.fila, destino.columna]?.Pieza;

            // Simular el movimiento: mover la pieza de origen a destino
            tableroArray[destino.fila, destino.columna] = origen;
            tableroArray[origen.GetPosicionCasillaEnTablero().fila, origen.GetPosicionCasillaEnTablero().columna] = null;

            // Encontrar la posición del rey del jugador que está moviendo
            var rey = GetPosicionRey(origen.Pieza.GetColor());

            // Verificar si el rey está en jaque después del movimiento simulado
            bool resultado = !EstaEnJaque(rey);

            // Restaurar el tablero a su estado original
            tableroArray[origen.GetPosicionCasillaEnTablero().fila, origen.GetPosicionCasillaEnTablero().columna] = origen;
            tableroArray[destino.fila, destino.columna].Pieza = piezaOriginal;

            return resultado;
        }

        //Vamos a definir un método para saber si el rey está en jaque mate
        public bool EstaEnJaqueMate()
        {
            var rey = GetPosicionRey(Turno % 2 == 0 ? "Negra" : "Blanca");
            if(!EstaEnJaque(rey))
            {
                return false;
            }

            // Obtén todos los movimientos posibles del rey
            var movimientosRey = rey.Pieza.GetMovimientos(rey.GetPosicionCasillaEnTablero().fila, rey.GetPosicionCasillaEnTablero().columna, ObtenerTablero());

            // Verifica si algún movimiento del rey lo saca del jaque
            foreach (var movimiento in movimientosRey)
            {
                if (PuedeMoverYNoQuedarEnJaque(rey, movimiento))
                {
                    return false; // Hay un movimiento que saca al rey del jaque
                }
            }

            // Verifica si alguna otra pieza puede proteger al rey
            foreach (var casilla in Tablero.Children.OfType<Casillas>())
            {
                if (casilla.Pieza != null && casilla.Pieza.GetColor() == (Turno % 2 == 0 ? "Negra" : "Blanca"))
                {
                    var movimientos = casilla.Pieza.GetMovimientos(casilla.GetPosicionCasillaEnTablero().fila, casilla.GetPosicionCasillaEnTablero().columna, ObtenerTablero());
                    foreach (var movimiento in movimientos)
                    {
                        if (PuedeMoverYNoQuedarEnJaque(casilla, movimiento))
                        {
                            return false; // Hay un movimiento que protege al rey
                        }
                    }
                }
            }

            return true; // No hay movimientos posibles, es jaque mate


        }
    }
}