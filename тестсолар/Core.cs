namespace тестсолар;

public class Core
{
    public Core()
    {
        SelectNearest(); 
        Menu();
    }

    void Menu()
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\nВыберите пункт меню:" +
                              "\n1. Отобразить список всех дней рождения" +
                              "\n2. Отобразить ближайшие и сегодняшние дни рождения" +
                              "\n3. Добавить запись" +
                              "\n4. Удалить запись" +
                              "\n5. Редактировать запись" +
                              "\n0. Закрыть программу");
            Console.ResetColor();
            int userChoice = new int();
            try
            {
                 userChoice = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Некорректное значение");
               continue;
            }

            switch (userChoice)
            {
                case 1:
                    SelectAll();
                    break;
              case 2:
                    SelectNearest();
                    break;
                case 3:
                    AddValue();
                    break;
                case 4:
                    RemoveValue();
                    break;
                case 5:
                    EditValue();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Вы ввели неверный пункт меню.");
                    break;
            }
        }

    }

    void SelectAll()//1 все др
    {
        using var db = new ApplicationContext();
        
        var people = db.Persons.ToList();
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("Список дней рождений:");
        Console.ResetColor();
        
        foreach (var u in people)
        {
            Console.WriteLine($"{u.Id}.{u}  ");
        }
    }
    
    void SelectNearest()//2 ближайшие и сегодняшние
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("Сегодняшние и ближайшие дни рождения:");
        Console.ResetColor();
        
        using ApplicationContext db = new ApplicationContext();
        
        var people = db.Persons.ToList();
        var result = people.Select(person =>
        {
            person.nearestBirthday =
                new DateTime(IsDateAfter(person.date, DateTime.Today) ? DateTime.Today.Year : DateTime.Today.Year + 1,
                    person.date.Month, person.date.Day);
            return person;
        })
            .Where(person => person.nearestBirthday <= DateTime.Today.AddDays(5))
            .OrderBy(person => person.nearestBirthday);
        
        foreach (var person in result)
        {
            Console.WriteLine($"{person}");
        }
    }

    void AddValue()//3 добавить значения
    {
        Console.WriteLine("Введите имя:");
        var name = Console.ReadLine();
        Console.WriteLine("Введите дату:");

        DateTime isDateValid = new DateTime();
        try
        {
            isDateValid = DateTime.Parse(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("Вы ввели некорректную дату");
        }
        using var db = new ApplicationContext();
        
        var newPerson = new Person { name = name, date = isDateValid };
        db.Persons.Add(newPerson);
        db.SaveChanges();
        Console.WriteLine("Данные успешно сохранены");
    }
    
    void RemoveValue()// 4 удалить запись
    {
        using var db = new ApplicationContext();   

        Console.WriteLine("Введите номер");
        int n = new int();
        try 
        {
             n = int.Parse(Console.ReadLine()); 

        } 
        catch(Exception e)
        {
            Console.WriteLine("Вы ввели некорректный номер");
            return;
        }  

        var person = db.Persons.Find(n);
        if (person != null)
        {
            db.Persons.Remove(person);
            db.SaveChanges();
            return;
        }
            Console.WriteLine("Такого номера не существует");
    }

    void EditValue()//5 редактировать запись
    {
        using var db = new ApplicationContext();
        
        Console.WriteLine("Введите номер");
        int n = new int();
        try
        {
            n = int.Parse(Console.ReadLine());
        }
        catch (Exception e)
        {
            Console.WriteLine("Вы ввели некорректный номер");
            return;
        }

        var person = db.Persons.Find(n);
        if (person == null)
        {
            Console.WriteLine("Такого номера не существует");
            return;
        }

        Console.WriteLine("\nЧто Вы хотите сделать?" +
                          "\n1. Изменить имя" +
                          "\n2. Изменить дату" +
                          "\n3. Изменить имя и дату");
        int userChoice = new int();
        try
        {
            userChoice = Convert.ToInt32(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("Вы ввели некорректный номер");
        }
        
        switch (userChoice)
        {
            case 1:
                Console.WriteLine("Введите имя");
                person.name = Console.ReadLine();
                break;
            case 2:
                Console.WriteLine("Введите дату");
                try
                {
                    person.date = DateTime.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Вы ввели некорректное значение");
                }     
                break;
            case 3:
                Console.WriteLine("Введите имя");
                person.name = Console.ReadLine();
                Console.WriteLine("Введите дату");
                try
                {
                    person.date = DateTime.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Вы ввели некорректное значение");
                }
                break;
        }
        db.SaveChanges();
    }
    

    bool IsDateAfter(DateTime a, DateTime b)
        {
            if (a.Month > b.Month)
            {
                return true;
            }

            if (a.Month == b.Month)
            {
                return a.Day.CompareTo(b.Day) >= 0;
            }

            return a.Month.CompareTo(b.Month) > 0;
        }
}