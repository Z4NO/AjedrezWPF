using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace AjedrezWPF
{
    public partial class MainWindow : Window
    {
        private const int filas = 8;
        private const int columnas = 8;
        private Casillas[,] tablero = new Casillas[filas, columnas];
        Grid tableroGrid = new Grid();

        public MainWindow()
        {
            InitializeComponent();

            // Ajustamos el ancho del tablero a 600 de anchura y 600 de altura
            tableroGrid.Width = 630;
            tableroGrid.Height = 600;
            // Ajustamos el tablero para que aparezca a la izquierda de la ventana principal
            tableroGrid.HorizontalAlignment = HorizontalAlignment.Left;

            GenerarTablero();
            this.ResizeMode = ResizeMode.NoResize;
            turnoLabel.Visibility = Visibility.Visible;
            turnoLabel.Foreground = Brushes.Black;
            turnoLabel.FontSize = 20;
            turnoLabel.Content = "Turno: Blancas";

            // Agregar el tablero al Grid secundario (GridTablero)
            GridTablero.Children.Add(tableroGrid);
        }

        private void GenerarTablero()
        {
            // Crear filas y columnas en el Grid
            for (int i = 0; i < filas; i++)
            {
                tableroGrid.RowDefinitions.Add(new RowDefinition());
                tableroGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Crear las casillas
            for (int fila = 0; fila < filas; fila++)
            {
                for (int columna = 0; columna < columnas; columna++)
                {
                    var casilla = new Casillas(fila, columna, tableroGrid, turnoLabel, PeonesBlancasLabel, TorresBlancasLabel, AlfilBlancasLabel, CaballoBlancasLabel, PeonesLabel, TorresLabel, AlfilLabel, CaballoLabel);
                    tablero[fila, columna] = casilla;

                    // Suscribirse al evento Click
                    casilla.Click += casilla.Casilla_Click;

                    // Posicionar en el Grid
                    Grid.SetRow(casilla, fila);
                    Grid.SetColumn(casilla, columna);
                    tableroGrid.Children.Add(casilla);
                }
            }

            // Agregar piezas al tablero (ejemplo)
            tablero[0, 0].AgregarPieza(new Pieza(true, false, "Torre")); // TORRE
            tablero[0, 1].AgregarPieza(new Pieza(true, false, "Caballo"));  // CABALLO
            tablero[0, 2].AgregarPieza(new Pieza(true, false, "Alfil")); // ALFIL
            tablero[0, 3].AgregarPieza(new Pieza(true, false, "Reina")); // REINA 
            tablero[0, 4].AgregarPieza(new Pieza(true, false, "Rey")); // REY
            tablero[0, 5].AgregarPieza(new Pieza(true, false, "Alfil")); // ALFIL
            tablero[0, 6].AgregarPieza(new Pieza(true, false, "Caballo")); // CABALLO
            tablero[0, 7].AgregarPieza(new Pieza(true, false, "Torre")); // TORRE

            for (int columna = 0; columna < columnas; columna++)
            {
                tablero[1, columna].AgregarPieza(new Pieza(true, false, "Peón")); // PEÓN blanco
                tablero[6, columna].AgregarPieza(new Pieza(false, true, "Peón")); // PEÓN negro
            }

            tablero[7, 0].AgregarPieza(new Pieza(false, true, "Torre"));
            tablero[7, 1].AgregarPieza(new Pieza(false, true, "Caballo"));
            tablero[7, 2].AgregarPieza(new Pieza(false, true, "Alfil"));
            tablero[7, 3].AgregarPieza(new Pieza(false, true, "Reina")); // REINA
            tablero[7, 4].AgregarPieza(new Pieza(false, true, "Rey"));  // REY
            tablero[7, 5].AgregarPieza(new Pieza(false, true, "Alfil"));
            tablero[7, 6].AgregarPieza(new Pieza(false, true, "Caballo"));
            tablero[7, 7].AgregarPieza(new Pieza(false, true, "Torre"));
        }
    }
}