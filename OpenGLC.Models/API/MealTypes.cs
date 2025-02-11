
namespace OpenGLC.Models.API
{
    public class MealTypes
    {
        public MealTypes()
        {
            Meals = new List<MealNameType>();
        }
        public static List<MealNameType> Meals;

        public static List<MealNameType> GetMealTypesDefinition()
        {
            Meals =
            [
				new MealNameType(-1, "Sin comida"),
				new MealNameType(0, "Desayuno"),
                new MealNameType(1, "Almuerzo"),
                new MealNameType(2, "Comida"),
                new MealNameType(3, "Merienda"),
                new MealNameType(4, "Cena"),
				new MealNameType(5, "Snack"),


			];

            return Meals;
        }
    }

    public class MealNameType
    {
        public MealNameType(int type, string name)
        {
            Type = type;
            Name = name;
        }
        public int Type { get; set; }
        public string Name { get; set; }
    }
}
