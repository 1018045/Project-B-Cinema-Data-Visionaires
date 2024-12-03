public class JobVacancyLogic
{
    public List<JobVacancy> Vacancies { get; private set; }
    public List<JobApplication> Applications { get; private set; }

    public JobVacancyLogic()
    {
        Vacancies = JobVacancyAccess.LoadAll();
        Applications = JobApplicationAccess.LoadAll();
    }

    public void AddApplication(int vacancyId, string email, string motivation, DateTime dateApplied)
    {
        Applications.Add(new JobApplication(FindNextAvailableApplicationId(), vacancyId, email, motivation, dateApplied));
        JobApplicationAccess.WriteAll(Applications);
    }

    public void AddVacancy(string jobTitle, string jobDescription, decimal? salary, string employmentType)
    {
        int newId = FindNextAvailableVacancyId();
        
        JobVacancy newVacancy = new JobVacancy(
            newId, 
            jobTitle, 
            jobDescription,
            DateTime.Now,
            salary,
            employmentType
        );

        Vacancies.Add(newVacancy);
        JobVacancyAccess.WriteAll(Vacancies);
    }

    public bool RemoveVacancy(int id)
    {
        foreach (var vacancy in Vacancies)
        {
            if (vacancy.VacancyId == id)
            {
                Vacancies.Remove(vacancy);
                JobVacancyAccess.WriteAll(Vacancies);
                return true;
            }
        }
        return false;
    }

    private int FindNextAvailableApplicationId()
    {
        int pointer = 0;
        foreach (JobApplication application in Applications.OrderBy(a => a.ApplicationId))
        {
            if (application.ApplicationId != pointer++)
            {
                return pointer;
            }
        }
        return pointer;     
    }

    private int FindNextAvailableVacancyId()
    {
        int pointer = 0;
        foreach (JobVacancy vacancy in Vacancies.OrderBy(v => v.VacancyId))
        {
            if (vacancy.VacancyId != pointer++)
            {
                return pointer;
            }
        }
        return pointer;     
    }

    public string ShowAllVacancies()
    {
        Vacancies = JobVacancyAccess.LoadAll();
        
        if (Vacancies.Count == 0)
        {
            return "No vacancies available at the moment.\n";
        }

        string output = "";
        foreach (var vacancy in Vacancies)
        {
            output += $"ID: {vacancy.VacancyId}\n";
            output += $"Position: {vacancy.JobTitle}\n";
            output += $"Description: {vacancy.JobDescription}\n";
            output += $"Type: {vacancy.EmploymentType}\n";
            if (vacancy.Salary.HasValue)
            {
                output += "Salary: " + vacancy.Salary.Value.ToString("C") + "\n";
            }
            else
            {
                output += "Salary: Not specified\n";
            }

            output += "------------------------\n";
        }
        return output;
    }

    public JobVacancy GetJobVacancyById(int id)
    {
        return Vacancies.Where(v => v.VacancyId == id).First();
    }
}