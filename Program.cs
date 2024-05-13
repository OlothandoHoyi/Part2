using System;
using System.Collections.Generic;
using System.Linq;

// Define a delegate for notifying when a recipe exceeds 300 calories
public delegate void RecipeCaloryExceededEventHandler(object sender, EventArgs e);

// Ingredient class to store additional information
public class Ingredient
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public string Unit { get; set; }
    public int Calories { get; set; }
    public string FoodGroup { get; set; }
}

// Recipe class with additional features and requirements
public class Recipe
{
    public string Name { get; set; }
    public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    public List<string> Steps { get; set; } = new List<string>();

    // Event for notifying when a recipe exceeds 300 calories
    public event RecipeCaloryExceededEventHandler RecipeCaloryExceeded;

    // Method to enter a new recipe
    public void EnterRecipe()
    {
        Console.WriteLine("Enter recipe name:");
        Name = Console.ReadLine();

        while (true)
        {
            Ingredient ingredient = new Ingredient();
            Console.WriteLine("Enter ingredient name (or 'done' to finish):");
            string ingredientName = Console.ReadLine();
            if (ingredientName.ToLower() == "done") break;

            ingredient.Name = ingredientName;
            Console.WriteLine("Enter quantity:");
            ingredient.Quantity = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter unit of measurement:");
            ingredient.Unit = Console.ReadLine();
            Console.WriteLine("Enter calories:");
            ingredient.Calories = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter food group:");
            ingredient.FoodGroup = Console.ReadLine();

            Ingredients.Add(ingredient);
        }

        Console.WriteLine("Enter the number of steps:");
        int numberSteps = int.Parse(Console.ReadLine());
        for (int j = 0; j < numberSteps; j++)
        {
            Console.WriteLine("Enter description of step " + (j + 1) + ":");
            Steps.Add(Console.ReadLine());
        }

        // Check if total calories exceed 300 and notify if so
        if (TotalCalories() > 300)
        {
            RecipeCaloryExceeded?.Invoke(this, EventArgs.Empty);
        }
    }

    // Method to display the entered recipe
    public void DisplayRecipe()
    {
        Console.WriteLine("Recipe Name: " + Name);
        Console.WriteLine("Ingredients:");
        foreach (var ingredient in Ingredients)
        {
            Console.WriteLine(" - " + ingredient.Quantity + " " + ingredient.Unit + " of " + ingredient.Name +
                              " (" + ingredient.Calories + " calories, Food Group: " + ingredient.FoodGroup + ")");
        }
        Console.WriteLine("Steps:");
        for (int j = 0; j < Steps.Count; j++)
        {
            Console.WriteLine((j + 1) + ". " + Steps[j]);
        }
    }

    // Method to calculate total calories of the recipe
    public int TotalCalories()
    {
        return Ingredients.Sum(ingredient => ingredient.Calories);
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<Recipe> recipes = new List<Recipe>();

        while (true)
        {
            Console.WriteLine("1. Enter recipe");
            Console.WriteLine("2. Display recipes");
            Console.WriteLine("3. Exit");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    Recipe recipe = new Recipe();
                    recipe.RecipeCaloryExceeded += (sender, eventArgs) =>
                    {
                        Console.WriteLine("Warning: Total calories exceed 300!");
                    };
                    recipe.EnterRecipe();
                    recipes.Add(recipe);
                    break;
                case 2:
                    if (recipes.Count == 0)
                    {
                        Console.WriteLine("No recipes entered yet.");
                        break;
                    }

                    // Display recipes in alphabetical order by name
                    foreach (var r in recipes.OrderBy(r => r.Name))
                    {
                        Console.WriteLine("Recipe: " + r.Name);
                    }

                    // Select a recipe to display
                    Console.WriteLine("Enter the name of the recipe to display:");
                    string recipeName = Console.ReadLine();
                    Recipe selectedRecipe = recipes.FirstOrDefault(r => r.Name == recipeName);
                    if (selectedRecipe != null)
                    {
                        selectedRecipe.DisplayRecipe();
                    }
                    else
                    {
                        Console.WriteLine("Recipe not found.");
                    }
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 3.");
                    break;
            }
        }
    }
}

