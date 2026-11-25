
using System;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace ZoologicoPOO
{
    // ---------------------------------------------------------------- 8. ENUMS
    enum Dieta { Herbivoro, Carnivoro, Omnivoro }

    // Enum cambiando los valores de las constantes
    enum NivelPeligro { Bajo = 1, Medio = 5, Alto = 10 }

    // -------------------------------------------------------------- 9. STRUCTS
    struct Coordenada
    {
        public double X;
        public double Y;

        public Coordenada(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double DistanciaA(Coordenada otra)
        {
            double dx = X - otra.X;
            double dy = Y - otra.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }

    // ------------------------------------------------------- 6. CLASE ESTÁTICA
    static class Utilidades
    {
        private static string prefijo;

        // Constructor static: se ejecuta una sola vez, al primer uso de la clase
        static Utilidades()
        {
            prefijo = "[ZOO] ";
            Console.WriteLine("(Constructor static de Utilidades ejecutado una sola vez)");
        }

        // Métodos estáticos
        public static string Formatear(string texto)
        {
            return prefijo + texto;
        }

        public static void Titulo(string texto)
        {
            Console.WriteLine();
            Console.WriteLine("=== " + texto + " ===");
        }
    }

    // ---------------------------------------------------------- 7. INTERFACES
    interface IVolador
    {
        void Despegar();
        void Desplazar();   // coincide con INadador
    }

    interface INadador
    {
        void Desplazar();   // coincide con IVolador
    }

    // ------------------------------------------------------------ 5. HERENCIA
    abstract class Animal
    {
        private string nombre = "";     // private: solo dentro de Animal
        protected int energia;          // protected: visible en las hijas

        // Getter/setter forma completa
        public string Nombre
        {
            get { return nombre; }
            set { if (value != "") nombre = value; }
        }

        // Forma reducida
        public int Edad { get; set; }
        public Dieta Dieta { get; set; }
        public NivelPeligro Peligro { get; set; }

        public int Energia
        {
            get { return energia; }
        }

        // 4. CONSTRUCTORES
        public Animal() : this("Sin nombre", 0)
        {
        }

        public Animal(string nombre, int edad)
        {
            Nombre = nombre;
            Edad = edad;
            energia = 100;
            Dieta = Dieta.Omnivoro;
            Peligro = NivelPeligro.Bajo;
            AnunciarNacimiento();       // un constructor llama a otro método
        }

        // Constructor para copiar objetos
        public Animal(Animal otro) : this(otro.Nombre, otro.Edad)
        {
            Dieta = otro.Dieta;
            Peligro = otro.Peligro;
        }

        private void AnunciarNacimiento()
        {
            Console.WriteLine("   [Constructor Animal] Nace: " + Nombre);
        }

        // Método abstracto
        public abstract string Sonido();

        // Método virtual
        public virtual void Moverse()
        {
            energia = energia - 5;
            Console.WriteLine("   " + Nombre + " se mueve. Energía: " + energia);
        }

        // Método no virtual (la hija Gato lo oculta con "new")
        public void Describir()
        {
            Console.WriteLine("   [Describir de Animal] " + Nombre + ", " + Edad + " años.");
        }
    }

    class Perro : Animal
    {
        public Perro(string nombre, int edad) : base(nombre, edad)
        {
            Dieta = Dieta.Omnivoro;
            Peligro = NivelPeligro.Medio;
            Console.WriteLine("   [Constructor Perro]");
        }

        public Perro(Perro otro) : base(otro)
        {
            Console.WriteLine("   [Constructor de copia Perro]");
        }

        public override string Sonido()
        {
            return "¡Guau!";
        }

        public override void Moverse()
        {
            base.Moverse();     // llamando al método de la clase madre
            Console.WriteLine("   ...y además " + Nombre + " mueve la cola.");
        }

        public void Correr()
        {
            energia = energia - 20;     // protected sí se puede usar en la hija
            Console.WriteLine("   " + Nombre + " corre. Energía: " + energia);
            // Console.WriteLine(nombre);   // Error: 'nombre' es private en Animal
        }
    }

    // sealed: evita la herencia
    sealed class Gato : Animal
    {
        public Gato(string nombre, int edad) : base(nombre, edad)
        {
            Dieta = Dieta.Carnivoro;
        }

        public override string Sonido()
        {
            return "¡Miau!";
        }

        // Método igual en madre e hija
        public new void Describir()
        {
            Console.WriteLine("   [Describir de Gato] " + Nombre + " duerme 16 horas al día.");
        }
    }

    class Leon : Animal
    {
        public Leon(string nombre, int edad) : base(nombre, edad)
        {
            Dieta = Dieta.Carnivoro;
            Peligro = NivelPeligro.Alto;
        }

        public override string Sonido()
        {
            return "¡Roaar!";
        }
    }

    // Herencia + interface múltiple
    class Pato : Animal, IVolador, INadador
    {
        public Pato(string nombre, int edad) : base(nombre, edad)
        {
        }

        public override string Sonido()
        {
            return "¡Cuac!";
        }

        public void Despegar()
        {
            Console.WriteLine("   " + Nombre + " despega.");
        }

        // Implementación explícita para resolver la ambigüedad
        void IVolador.Desplazar()
        {
            Console.WriteLine("   " + Nombre + " vuela por el cielo.");
        }

        void INadador.Desplazar()
        {
            Console.WriteLine("   " + Nombre + " nada en el lago.");
        }
    }

    // ------------------------- 1, 3, 4, 11. CUIDADOR (y destructor)
    class Cuidador
    {
        private string nombre = "";
        private string turno = "Mañana";

        // Getters y setters como métodos
        public string GetTurno()
        {
            return turno;
        }

        public void SetTurno(string nuevoTurno)
        {
            if (nuevoTurno == "Mañana" || nuevoTurno == "Tarde" || nuevoTurno == "Noche")
            {
                turno = nuevoTurno;
            }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value.Trim(); }
        }

        public int Edad { get; set; }

        public Cuidador()
        {
            Nombre = "Anónimo";
        }

        public Cuidador(string nombre, int edad)
        {
            Nombre = nombre;
            Edad = edad;
        }

        // 3. SOBRECARGA de métodos
        // Por número de parámetros
        public void Alimentar(double kilos)
        {
            Console.WriteLine("   " + Nombre + " reparte " + kilos + " kg de comida.");
        }

        public void Alimentar(Animal animal, double kilos)
        {
            Console.WriteLine("   " + Nombre + " le da " + kilos + " kg a " + animal.Nombre + ".");
        }

        // Por tipo de parámetro
        public void Alimentar(string comida)
        {
            Console.WriteLine("   " + Nombre + " reparte " + comida + ".");
        }

        // 11. DESTRUCTOR
        ~Cuidador()
        {
            Console.WriteLine("   [Destructor] Se libera el cuidador " + nombre);
        }
    }

    // ------------------------------------------------------ 10. CLASES PARCIALES
    partial class Zoologico
    {
        private Animal[] animales = new Animal[10];
        private int cantidad = 0;
        public string Nombre { get; set; }

        public Zoologico(string nombre)
        {
            Nombre = nombre;
        }

        public void Agregar(Animal animal)
        {
            animales[cantidad] = animal;
            cantidad++;
        }
    }

    partial class Zoologico
    {
        public int Cantidad
        {
            get { return cantidad; }
        }

        public Animal Obtener(int indice)
        {
            return animales[indice];
        }

        public void Listar()
        {
            Console.WriteLine("   Animales de " + Nombre + ":");
            for (int i = 0; i < cantidad; i++)
            {
                Console.WriteLine("    - " + animales[i].Nombre + " (" + animales[i].Edad + " años)");
            }
        }
    }

    // ------------------------------------------------- 12. PATRONES DE DISEÑO

    // Factory Method
    abstract class CreadorAnimal
    {
        public abstract Animal Crear(string nombre, int edad);
    }

    class CreadorPerro : CreadorAnimal
    {
        public override Animal Crear(string nombre, int edad) { return new Perro(nombre, edad); }
    }

    class CreadorGato : CreadorAnimal
    {
        public override Animal Crear(string nombre, int edad) { return new Gato(nombre, edad); }
    }

    class CreadorLeon : CreadorAnimal
    {
        public override Animal Crear(string nombre, int edad) { return new Leon(nombre, edad); }
    }

    // Abstract Factory
    interface IAlimento { string Nombre(); }
    interface IRefugio { string Descripcion(); }

    class Carne : IAlimento { public string Nombre() { return "Carne fresca"; } }
    class Heno : IAlimento { public string Nombre() { return "Heno seco"; } }
    class Cueva : IRefugio { public string Descripcion() { return "una cueva rocosa"; } }
    class Establo : IRefugio { public string Descripcion() { return "un establo de madera"; } }

    interface IFabricaHabitat
    {
        IAlimento CrearAlimento();
        IRefugio CrearRefugio();
    }

    class FabricaSabana : IFabricaHabitat
    {
        public IAlimento CrearAlimento() { return new Carne(); }
        public IRefugio CrearRefugio() { return new Cueva(); }
    }

    class FabricaGranja : IFabricaHabitat
    {
        public IAlimento CrearAlimento() { return new Heno(); }
        public IRefugio CrearRefugio() { return new Establo(); }
    }

    // Singleton
    sealed class RegistroZoo
    {
        private static RegistroZoo? instancia;
        private int total = 0;
        private string ultimoEvento = "";

        private RegistroZoo() { }

        public static RegistroZoo Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new RegistroZoo();
                }
                return instancia;
            }
        }

        public void Registrar(string evento)
        {
            total++;
            ultimoEvento = evento;
        }

        public void Mostrar()
        {
            Console.WriteLine("   Eventos registrados: " + total + ". Último: " + ultimoEvento);
        }
    }

    // Builder
    class FichaAnimal
    {
        public string Nombre = "";
        public string Especie = "";
        public Dieta Dieta;
        public string Habitat = "";
    }

    class FichaBuilder
    {
        private FichaAnimal ficha = new FichaAnimal();

        public FichaBuilder ConNombre(string nombre) { ficha.Nombre = nombre; return this; }
        public FichaBuilder ConEspecie(string especie) { ficha.Especie = especie; return this; }
        public FichaBuilder ConDieta(Dieta dieta) { ficha.Dieta = dieta; return this; }
        public FichaBuilder ConHabitat(string habitat) { ficha.Habitat = habitat; return this; }
        public FichaAnimal Construir() { return ficha; }
    }

    // Adapter
    class SensorFahrenheit
    {
        public double LeerFahrenheit() { return 95.0; }
    }

    interface ITermometroCelsius
    {
        double LeerCelsius();
    }

    class AdaptadorSensor : ITermometroCelsius
    {
        private SensorFahrenheit sensor;

        public AdaptadorSensor(SensorFahrenheit sensor)
        {
            this.sensor = sensor;
        }

        public double LeerCelsius()
        {
            return (sensor.LeerFahrenheit() - 32) * 5.0 / 9.0;
        }
    }

    // Composite
    interface IComponenteZoo
    {
        int ContarAnimales();
        void Mostrar(int nivel);
    }

    class HojaAnimal : IComponenteZoo
    {
        private Animal animal;

        public HojaAnimal(Animal animal) { this.animal = animal; }

        public int ContarAnimales() { return 1; }

        public void Mostrar(int nivel)
        {
            Console.Write("   ");
            for (int i = 0; i < nivel; i++) Console.Write("   ");
            Console.WriteLine("- " + animal.Nombre);
        }
    }

    class SectorZoo : IComponenteZoo
    {
        private string nombre;
        private IComponenteZoo[] hijos = new IComponenteZoo[10];
        private int cantidad = 0;

        public SectorZoo(string nombre) { this.nombre = nombre; }

        public void Agregar(IComponenteZoo componente)
        {
            hijos[cantidad] = componente;
            cantidad++;
        }

        public int ContarAnimales()
        {
            int total = 0;
            for (int i = 0; i < cantidad; i++) total = total + hijos[i].ContarAnimales();
            return total;
        }

        public void Mostrar(int nivel)
        {
            Console.Write("   ");
            for (int i = 0; i < nivel; i++) Console.Write("   ");
            Console.WriteLine("[" + nombre + "] (" + ContarAnimales() + " animales)");
            for (int i = 0; i < cantidad; i++) hijos[i].Mostrar(nivel + 1);
        }
    }

    // Facade
    class ZoologicoFacade
    {
        private Zoologico zoo;
        private ITermometroCelsius termometro;

        public ZoologicoFacade(Zoologico zoo, ITermometroCelsius termometro)
        {
            this.zoo = zoo;
            this.termometro = termometro;
        }

        public void AbrirDia()
        {
            Console.WriteLine("   Abriendo " + zoo.Nombre + "...");
            Console.WriteLine("   Temperatura: " + termometro.LeerCelsius().ToString("F1") + " °C");
            for (int i = 0; i < zoo.Cantidad; i++)
            {
                Console.WriteLine("   " + zoo.Obtener(i).Nombre + " saluda: " + zoo.Obtener(i).Sonido());
            }
            RegistroZoo.Instancia.Registrar("Se abrió el día con " + zoo.Cantidad + " animales");
        }
    }

    // Modelo Vista Controlador
    class ModeloAnimal
    {
        public string Nombre = "";
        public int Energia = 50;
    }

    class VistaAnimal
    {
        public void Mostrar(ModeloAnimal m)
        {
            Console.WriteLine("   [Vista] " + m.Nombre + " tiene energía " + m.Energia + "/100");
        }
    }

    class ControladorAnimal
    {
        private ModeloAnimal modelo;
        private VistaAnimal vista;

        public ControladorAnimal(ModeloAnimal modelo, VistaAnimal vista)
        {
            this.modelo = modelo;
            this.vista = vista;
        }

        public void Alimentar(int puntos)
        {
            modelo.Energia = modelo.Energia + puntos;
            if (modelo.Energia > 100) modelo.Energia = 100;
            vista.Mostrar(modelo);
        }
    }

 
    interface ITarifa
    {
        double Calcular(double precioBase);
    }

    class TarifaNormal : ITarifa
    {
        public double Calcular(double precioBase) { return precioBase; }
    }

    class TarifaNino : ITarifa
    {
        public double Calcular(double precioBase) { return precioBase * 0.5; }
    }

    class TarifaJubilado : ITarifa
    {
        public double Calcular(double precioBase) { return precioBase * 0.7; }
    }

    class Taquilla
    {
        private ITarifa tarifa;

        public Taquilla(ITarifa tarifa) { this.tarifa = tarifa; }

        public double Cobrar(int personas, double precioBase)
        {
            return personas * tarifa.Calcular(precioBase);
        }
    }

    // ----------------------------------------------------------- 15. DELEGADOS
    delegate double Operacion(double valor);
    delegate void Notificador(string mensaje);

    // ------------------------------------------------------- 16. PERSISTENCIA
    public class AnimalDto
    {
        public string Nombre { get; set; } = "";
        public int Edad { get; set; }
        public string Especie { get; set; } = "";
    }

    // ------------------------------------------------------------ 17. GENÉRICOS
    class Jaula<T> where T : Animal
    {
        private T[] ocupantes = new T[10];
        private int cantidad = 0;

        public void Meter(T animal)
        {
            ocupantes[cantidad] = animal;
            cantidad++;
        }

        public T Obtener(int indice) { return ocupantes[indice]; }

        public int Cantidad
        {
            get { return cantidad; }
        }
    }

    static class Genericos
    {
        public static void Intercambiar<T>(ref T a, ref T b)
        {
            T temporal = a;
            a = b;
            b = temporal;
        }
    }

    // ============================================================ PROGRAMA
    class Program
    {
        static double Duplicar(double v) { return v * 2; }
        static double Cuadrado(double v) { return v * v; }
        static void AvisarPorConsola(string m) { Console.WriteLine("   [Consola] " + m); }
        static void AvisarPorRegistro(string m) { RegistroZoo.Instancia.Registrar("Aviso: " + m); }

        static void CrearYSoltarCuidador()
        {
            Cuidador temporal = new Cuidador("Carlos (temporal)", 22);
            Console.WriteLine("   Se creó " + temporal.Nombre + " y ahora se suelta.");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Antes de usar Utilidades (el constructor static aún no corrió)");

            // 1. Clases, getters y setters
            Utilidades.Titulo("1. Clases, atributos privados, getters y setters");
            Cuidador ana = new Cuidador { Nombre = "  Ana  ", Edad = 30 };   // llama los setters
            ana.SetTurno("Tarde");
            ana.SetTurno("Madrugada");      // inválido, se ignora
            Console.WriteLine("   Cuidador: [" + ana.Nombre + "], edad " + ana.Edad + ", turno " + ana.GetTurno());
            Cuidador luis = new Cuidador("Luis", 45);
            Console.WriteLine("   Cuidador: " + luis.Nombre + ", edad " + luis.Edad);

            // 2. Dos variables, mismo objeto
            Utilidades.Titulo("2. Dos variables refiriendo al mismo objeto");
            Cuidador alias = ana;
            alias.Nombre = "Ana María";
            Console.WriteLine("   ana.Nombre = " + ana.Nombre);

            // 3. Sobrecarga
            Utilidades.Titulo("3. Sobrecarga de métodos");
            ana.Alimentar(10.5);
            ana.Alimentar(new Gato("Misu", 2), 0.3);
            ana.Alimentar("croquetas");

            // 4. Constructores
            Utilidades.Titulo("4. Constructores");
            Perro rex = new Perro("Rex", 3);
            Perro copia = new Perro(rex);
            copia.Nombre = "Rex II";
            Console.WriteLine("   Original: " + rex.Nombre + " | Copia: " + copia.Nombre);

            // 5. Herencia
            Utilidades.Titulo("5. Herencia, abstract, protected, virtual/override");
            Perro firulais = new Perro("Firulais", 5);
            firulais.Moverse();
            firulais.Correr();
            Console.WriteLine("   Energía (solo lectura): " + firulais.Energia);
            // Animal a = new Animal();     // Error: clase abstracta, no se instancia

            Gato michi = new Gato("Michi", 4);
            michi.Describir();                  // el de Gato
            ((Animal)michi).Describir();        // el de Animal
            // class Siames : Gato { }          // Error: Gato es sealed

            Console.WriteLine("   Polimorfismo:");
            Animal[] poli = new Animal[3];
            poli[0] = new Perro("Toby", 2);
            poli[1] = new Gato("Luna", 3);
            poli[2] = new Leon("Simba", 6);
            for (int i = 0; i < poli.Length; i++)
            {
                Console.WriteLine("   " + poli[i].Nombre + " dice " + poli[i].Sonido());
                poli[i].Moverse();
            }

            // 6. Estáticos
            Utilidades.Titulo("6. Métodos estáticos");
            Console.WriteLine("   " + Utilidades.Formatear("Bienvenidos al zoológico"));
            Console.WriteLine("   " + Utilidades.Formatear("El constructor static no se repite"));

            // 7. Interfaces
            Utilidades.Titulo("7. Interfaces");
            Pato donald = new Pato("Donald", 2);
            donald.Despegar();
            IVolador volador = donald;
            INadador nadador = donald;
            volador.Desplazar();
            nadador.Desplazar();

            // 8. Enums
            Utilidades.Titulo("8. Enums");
            Leon simba = new Leon("Simba", 6);
            Console.WriteLine("   Dieta: " + simba.Dieta + " (valor " + (int)simba.Dieta + ")");
            Console.WriteLine("   Peligro: " + simba.Peligro + " (valor " + (int)simba.Peligro + ")");
            Console.WriteLine("   NivelPeligro.Medio vale " + (int)NivelPeligro.Medio);

            // 9. Structs
            Utilidades.Titulo("9. Structs");
            Coordenada entrada = new Coordenada(0, 0);
            Coordenada jaulaLeones = new Coordenada(3, 4);
            Coordenada copiaStruct = jaulaLeones;   // se copia el valor
            copiaStruct.X = 100;
            Console.WriteLine("   Original: " + jaulaLeones + " | Copia: " + copiaStruct);
            Console.WriteLine("   Distancia entrada -> jaula: " + entrada.DistanciaA(jaulaLeones));

            // 10. Clases parciales
            Utilidades.Titulo("10. Clases parciales");
            Zoologico zoo = new Zoologico("Zoo Central");
            zoo.Agregar(new Perro("Rocky", 4));
            zoo.Agregar(new Gato("Nina", 2));
            zoo.Agregar(simba);
            zoo.Agregar(donald);
            zoo.Listar();

            // 11. Destructores
            Utilidades.Titulo("11. Destructores");
            CrearYSoltarCuidador();
            GC.Collect();                       // forzamos al recolector solo para demostrar
            GC.WaitForPendingFinalizers();

            // 12. Patrones de diseño
            Utilidades.Titulo("12. Patrones de diseño");

            Console.WriteLine(" - Factory Method:");
            CreadorAnimal creador = new CreadorGato();
            Animal creado = creador.Crear("Felix", 1);
            Console.WriteLine("   La fábrica creó un " + creado.GetType().Name + " que dice " + creado.Sonido());
            creador = new CreadorLeon();
            creado = creador.Crear("Nala", 3);
            Console.WriteLine("   La fábrica creó un " + creado.GetType().Name + " que dice " + creado.Sonido());

            Console.WriteLine(" - Abstract Factory:");
            IFabricaHabitat fabrica = new FabricaSabana();
            Console.WriteLine("   " + fabrica.CrearAlimento().Nombre() + " y " + fabrica.CrearRefugio().Descripcion());
            fabrica = new FabricaGranja();
            Console.WriteLine("   " + fabrica.CrearAlimento().Nombre() + " y " + fabrica.CrearRefugio().Descripcion());

            Console.WriteLine(" - Singleton:");
            RegistroZoo r1 = RegistroZoo.Instancia;
            RegistroZoo r2 = RegistroZoo.Instancia;
            r1.Registrar("Llegó un nuevo animal");
            Console.WriteLine("   ¿Mismo objeto? " + (r1 == r2));

            Console.WriteLine(" - Builder:");
            FichaAnimal ficha = new FichaBuilder()
                .ConNombre("Simba")
                .ConEspecie("León africano")
                .ConDieta(Dieta.Carnivoro)
                .ConHabitat("Sabana")
                .Construir();
            Console.WriteLine("   " + ficha.Nombre + " | " + ficha.Especie + " | " + ficha.Dieta + " | " + ficha.Habitat);

            Console.WriteLine(" - Adapter:");
            ITermometroCelsius termometro = new AdaptadorSensor(new SensorFahrenheit());
            Console.WriteLine("   Lectura adaptada: " + termometro.LeerCelsius().ToString("F1") + " °C");

            Console.WriteLine(" - Composite:");
            SectorZoo granja = new SectorZoo("Granja");
            granja.Agregar(new HojaAnimal(new Perro("Rocky", 4)));
            granja.Agregar(new HojaAnimal(new Gato("Nina", 2)));
            SectorZoo sabana = new SectorZoo("Sabana");
            sabana.Agregar(new HojaAnimal(simba));
            SectorZoo todo = new SectorZoo("Zoológico");
            todo.Agregar(granja);
            todo.Agregar(sabana);
            todo.Mostrar(0);

            Console.WriteLine(" - Facade:");
            ZoologicoFacade fachada = new ZoologicoFacade(zoo, termometro);
            fachada.AbrirDia();
            RegistroZoo.Instancia.Mostrar();

            Console.WriteLine(" - Modelo Vista Controlador:");
            ModeloAnimal modelo = new ModeloAnimal();
            modelo.Nombre = "Simba";
            modelo.Energia = 60;
            ControladorAnimal controlador = new ControladorAnimal(modelo, new VistaAnimal());
            controlador.Alimentar(25);
            controlador.Alimentar(40);

            // 13. SOLID
            Utilidades.Titulo("13. SOLID");
            Taquilla t1 = new Taquilla(new TarifaNormal());
            Taquilla t2 = new Taquilla(new TarifaNino());
            Taquilla t3 = new Taquilla(new TarifaJubilado());
            Console.WriteLine("   3 entradas normales:  $" + t1.Cobrar(3, 20000));
            Console.WriteLine("   3 entradas de niño:   $" + t2.Cobrar(3, 20000));
            Console.WriteLine("   3 entradas jubilado:  $" + t3.Cobrar(3, 20000));

            // 14. Arreglos de objetos
            Utilidades.Titulo("14. Arreglos de objetos");
            Animal[] arreglo = new Animal[5];
            string[] nombres = { "Zeta", "Alfa", "Gamma", "Beta", "Omega" };
            int[] edades = { 7, 2, 9, 4, 1 };

            // Llenado con ciclos
            for (int i = 0; i < arreglo.Length; i++)
            {
                if (i % 2 == 0) arreglo[i] = new Perro(nombres[i], edades[i]);
                else arreglo[i] = new Gato(nombres[i], edades[i]);
            }

            Console.WriteLine(" - Sin ordenar:");
            for (int i = 0; i < arreglo.Length; i++)
                Console.WriteLine("    " + arreglo[i].Nombre + " (" + arreglo[i].Edad + ")");

            // Ordenando por edad (método de la burbuja)
            for (int i = 0; i < arreglo.Length - 1; i++)
            {
                for (int j = 0; j < arreglo.Length - 1 - i; j++)
                {
                    if (arreglo[j].Edad > arreglo[j + 1].Edad)
                    {
                        Animal aux = arreglo[j];
                        arreglo[j] = arreglo[j + 1];
                        arreglo[j + 1] = aux;
                    }
                }
            }

            Console.WriteLine(" - Ordenado por edad:");
            for (int i = 0; i < arreglo.Length; i++)
                Console.WriteLine("    " + arreglo[i].Nombre + " (" + arreglo[i].Edad + ")");

            // 15. Delegados
            Utilidades.Titulo("15. Delegados");
            Operacion op = Duplicar;
            Console.WriteLine("   Duplicar(7) = " + op(7));
            op = Cuadrado;
            Console.WriteLine("   Cuadrado(7) = " + op(7));

            Notificador notificar = AvisarPorConsola;
            notificar += AvisarPorRegistro;     // delegado multicast
            notificar("Simulacro: un león se escapó");
            RegistroZoo.Instancia.Mostrar();

            // 16. Persistencia
            Utilidades.Titulo("16. Almacenamiento persistente");
            AnimalDto dto = new AnimalDto();
            dto.Nombre = simba.Nombre;
            dto.Edad = simba.Edad;
            dto.Especie = simba.GetType().Name;

            // JSON
            string rutaJson = Path.Combine(Path.GetTempPath(), "animal.json");
            string json = JsonSerializer.Serialize(dto);
            File.WriteAllText(rutaJson, json);
            string jsonLeido = File.ReadAllText(rutaJson);
            AnimalDto? desdeJson = JsonSerializer.Deserialize<AnimalDto>(jsonLeido);
            Console.WriteLine("   JSON: " + jsonLeido);
            if (desdeJson != null)
                Console.WriteLine("   Leído: " + desdeJson.Nombre + ", " + desdeJson.Edad + " años, " + desdeJson.Especie);

            // XML
            string rutaXml = Path.Combine(Path.GetTempPath(), "animal.xml");
            XmlSerializer serializador = new XmlSerializer(typeof(AnimalDto));
            StreamWriter escritor = new StreamWriter(rutaXml);
            serializador.Serialize(escritor, dto);
            escritor.Close();
            StreamReader lector = new StreamReader(rutaXml);
            AnimalDto? desdeXml = serializador.Deserialize(lector) as AnimalDto;
            lector.Close();
            if (desdeXml != null)
                Console.WriteLine("   XML leído: " + desdeXml.Nombre + ", " + desdeXml.Edad + " años, " + desdeXml.Especie);

            // 17. Genéricos
            Utilidades.Titulo("17. Genéricos");
            Jaula<Perro> jaulaPerros = new Jaula<Perro>();
            jaulaPerros.Meter(new Perro("Max", 3));
            jaulaPerros.Meter(new Perro("Bruno", 5));
            // jaulaPerros.Meter(new Gato("Tom", 2));   // Error: la jaula es solo de perros
            Console.WriteLine("   Jaula de perros: " + jaulaPerros.Cantidad + " ocupantes. Primero: " + jaulaPerros.Obtener(0).Nombre);

            int x = 10, y = 20;
            Genericos.Intercambiar(ref x, ref y);
            Console.WriteLine("   Intercambiar enteros: x=" + x + ", y=" + y);

            string s1 = "Hola", s2 = "Mundo";
            Genericos.Intercambiar(ref s1, ref s2);
            Console.WriteLine("   Intercambiar textos: s1=" + s1 + ", s2=" + s2);

            Console.WriteLine();
            Console.WriteLine("Fin del programa.");
        }
    }
}
