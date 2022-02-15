using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace SimulacroExamen
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BDTareasEntities1 bd = new BDTareasEntities1(); //instanciamos la BD 

        public MainWindow()
        {
            InitializeComponent();
            MostrarDatos(); //nadamas cargar la ventana principal, se muestra la tabla con los datos de la BD
            tareasDataGrid.IsReadOnly = true; //dataGrid de sola lectura 
            numID.IsReadOnly = true; //textBox ID de sola lectura
        }

        private void click_crearTarea(object sender, RoutedEventArgs e)
        {
            if(tareaTextBox.Text.Equals("")) //si el textBox está vacío, salta el error
            {
                MessageBox.Show("ERROR! Campo vacío.");
            }
            else
            {
                Tareas t = new Tareas();

                t.TASKNAME = tareaTextBox.Text; //se guarda en el campo taskname el text introducido
                bd.Tareas.Add(t); //se anyade la nueva tarea a la BD
                bd.SaveChanges();
                
                //hay que mostrar la BD en el DataGrid
                MostrarDatos();
                MessageBox.Show("Tarea añadida correctamente.");
            }
            Clear(); //se limpian los textboxes
        }

        private void MostrarDatos()
        {
            List<Tareas> listaTareas = bd.Tareas.ToList<Tareas>(); //creamos una lista de tareas
            tareasDataGrid.ItemsSource = listaTareas;   //que se mostrará en el dataGrid
            tareasDataGrid.Items.Refresh();   //refrescamos la vista del dataGrid
        }

        private void click_editarTarea(object sender, RoutedEventArgs e)
        {

            Tareas tarea = new Tareas();  //instancia de la tabla Tareas 
                
            if (tareaTextBox.Text.Equals("")) //si el campo está vacío, salta el error
            {
                MessageBox.Show("ERROR! Campo vacío.");
            }
            else if(tareasDataGrid.SelectedItem == null) //si no se ha seleccionado un elemento de la tabla
            {
                MessageBox.Show("ERROR! Para editar una tarea, primero seleccionala en la tabla.");
            }
            else //si no, se edita el campo del nombre de tarea con el nuevo nombre introducido en el textbox
            {
                tarea.TASKNAME = tareaTextBox.Text;
                tareasDataGrid.SelectedItem = null; //para quitar la seleccion del item que se quedaba seleccionado
                bd.SaveChanges();
                MessageBox.Show("Tarea editada correctamente.");
            }
            Clear(); //se limpian los textboxes
        }

        private void Clear()
        {
            tareaTextBox.Clear();
            numID.Clear();
        }

        private void click_borrarTarea(object sender, RoutedEventArgs e)
        {
            //buscamos la tarea que tenga el mismo ID que aperece en el textbox numID
            var tareaCompletada = (from t in bd.Tareas where 
                t.ID.ToString() == numID.Text select t).SingleOrDefault();

            if (tareaTextBox.Text.Equals("")) //si el campo está vacío, salta el error
            {
                MessageBox.Show("ERROR! Campo vacío.");
            }
            else if (tareasDataGrid.SelectedItem == null) //si no se ha seleccionado un elemento de la tabla
            {
                MessageBox.Show("ERROR! Para borrar una tarea, primero seleccionala en la tabla.");
            }
            else //si no se borra de la tabla Tareas la tarea completada
            {
                bd.Tareas.Remove(tareaCompletada);
                bd.SaveChanges();
                MostrarDatos(); //se actualiza
                MessageBox.Show("Enhorabuena! Tarea completada.");
            }
            Clear();
        }
    }
}
