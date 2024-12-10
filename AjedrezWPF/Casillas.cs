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


        public Casillas(int fila, int columna, Grid tablero, Label turno_labels, Label PeonesBlancasLabel, Label TorresBlancasLabel, Label AlfilsBlancasLabel, Label CaballosBlancasLabel, Label PeonesNegrasLabel,Label TooresNegrasLabel, Label AlfilsNegrasLabel, Label CaballosNegrasLabel ) : base()
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
            if (casillaSeleccionada == null)
            {
                if (HayPieza)
                {
                    casillaSeleccionada = this;
                    if (casillaSeleccionada.Pieza.GetColor().Equals("Blanca")  && Turno % 2 != 0)
                    {}
                    else if (casillaSeleccionada.Pieza.GetColor().Equals("Negra") && Turno % 2 == 0)
                    {}
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
    }
}