using System;
using System.Collections.Generic;

namespace PatronesDeDiseño.Builder.Conceptual
{
    // La interfaz Creador especifica métodos para crear las diferentes partes
    // de los objetos Producto.
    public interface ICreador
    {
        void ConstruirParteA();

        void ConstruirParteB();

        void ConstruirParteC();
    }

    // Las clases CreadorConcreto siguen la interfaz Creador y proporcionan
    // implementaciones específicas de los pasos de construcción. Su programa puede tener
    // varias variaciones de Creadores, implementados de manera diferente.
    public class CreadorConcreto : ICreador
    {
        private Producto _producto = new Producto();

        // Una instancia de creador nueva debería contener un objeto producto en blanco, que
        // se usa en el ensamblaje posterior.
        public CreadorConcreto()
        {
            this.Reiniciar();
        }

        public void Reiniciar()
        {
            this._producto = new Producto();
        }

        // Todos los pasos de producción trabajan con la misma instancia del producto.
        public void ConstruirParteA()
        {
            this._producto.Agregar("ParteA1");
        }

        public void ConstruirParteB()
        {
            this._producto.Agregar("ParteB1");
        }

        public void ConstruirParteC()
        {
            this._producto.Agregar("ParteC1");
        }

        // Los CreadoresConcretos deben proporcionar sus propios métodos para
        // recuperar resultados. Esto se debe a que diversos tipos de creadores pueden
        // crear productos completamente diferentes que no siguen la misma
        // interfaz. Por lo tanto, tales métodos no se pueden declarar en la base
        // Interfaz Creador (al menos en un lenguaje de programación
        // de tipo estático).
        //
        // Por lo general, después de devolver el resultado final al cliente, un creador
        // se espera que la instancia esté lista para comenzar a producir otro producto.
        // Por eso es una práctica habitual llamar al método de reinicio al final
        // del cuerpo del método `ObtenerProducto`. Sin embargo, este comportamiento no es
        // obligatorio, y puede hacer que sus creadores esperen una llamada de reinicio explícita
        // desde el código del cliente antes de desechar el resultado anterior.
        public Producto ObtenerProducto()
        {
            Producto resultado = this._producto;

            this.Reiniciar();

            return resultado;
        }
    }

    // Tiene sentido usar el patrón Creador solo cuando sus productos son
    // bastante complejos y requieren una configuración extensa.
    //
    // A diferencia de otros patrones de creación, diferentes creadores concretos pueden
    // producir productos no relacionados. En otras palabras, los resultados de varios creadores
    // no siempre pueden seguir la misma interfaz.
    public class Producto
    {
        private List<object> _partes = new List<object>();

        public void Agregar(string parte)
        {
            this._partes.Add(parte);
        }

        public string ListarPartes()
        {
            string cadena = string.Empty;

            for (int i = 0; i < this._partes.Count; i++)
            {
                cadena += this._partes[i] + ", ";
            }

            cadena = cadena.Remove(cadena.Length - 2); // eliminando la última ", "

            return "Partes del producto: " + cadena + "\n";
        }
    }

    // El Director solo es responsable de ejecutar los pasos de construcción en un
    // secuencia particular. Es útil cuando se producen productos de acuerdo con un
    // orden o configuración específica. En sentido estricto, la clase Director es
    // opcional, ya que el cliente puede controlar los creadores directamente.
    public class Director
    {
        private ICreador _creador;

        public ICreador Creador
        {
            set { _creador = value; }
        }

        // El Director puede construir varias variaciones de productos utilizando los mismos
        // pasos de construcción.
        public void ConstruirProductoMínimoViable()
        {
            this._creador.ConstruirParteA();
        }
        public void ConstruirProductoCompleto()
        {
            this._creador.ConstruirParteA();
            this._creador.ConstruirParteB();
            this._creador.ConstruirParteC();
        }
    }

    class Programa
    {
        static void Main(string[] args)
        {
            // El código del cliente crea un objeto creador, lo pasa al
            // director y luego inicia el proceso de construcción. El resultado final
            // se recupera del objeto creador.
            var director = new Director();
            var creador = new CreadorConcreto();
            director.Creador = creador;

            Console.WriteLine("Producto básico estándar:");
            director.ConstruirProductoMínimoViable();
            Console.WriteLine(creador.ObtenerProducto().ListarPartes());

            Console.WriteLine("Producto completo estándar:");
            director.ConstruirProductoCompleto();
            Console.WriteLine(creador.ObtenerProducto().ListarPartes());

            // Recuerda, el patrón Creador se puede usar sin una clase Director.
            Console.WriteLine("Producto personalizado:");
            creador.ConstruirParteA();
            creador.ConstruirParteC();
            Console.Write(creador.ObtenerProducto().ListarPartes());
        }
    }
}
