using System;
using System.Collections.Generic;
using System.Linq;

public class JobVacancyLogic
{
    private List<JobVacancy> _vacancies;

    public JobVacancyLogic()
    {
        _vacancies = JobVacancyAccess.LoadAll();
    }

    public void AddVacancy(string jobTitle, string jobDescription, decimal? salary, string employmentType)
    {
        int newId = FindNextAvailableId();
        
        JobVacancy newVacancy = new JobVacancy(
            newId, 
            jobTitle, 
            jobDescription,
            DateTime.Now,
            salary,
            employmentType
        );

        _vacancies.Add(newVacancy);
        JobVacancyAccess.WriteAll(_vacancies);
    }

    public bool RemoveVacancy(int id)
    {
        _vacancies = JobVacancyAccess.LoadAll();
        
        foreach (var vacancy in _vacancies)
        {
            if (vacancy.VacancyId == id)
            {
                _vacancies.Remove(vacancy);
                JobVacancyAccess.WriteAll(_vacancies);
                return true;
            }
        }
        return false;
    }

    private int FindNextAvailableId()
    {
        if (_vacancies.Count == 0)
        {
            return 1;
        }
        else
        {
            int highestId = 0;
                foreach (var vacancy in _vacancies)
                {
                    if (vacancy.VacancyId > highestId)
                    {
                        highestId = vacancy.VacancyId;
                    }
                }
            return highestId + 1;
        }
    }

    public string ShowAllVacancies()
    {
        _vacancies = JobVacancyAccess.LoadAll();
        
        if (_vacancies.Count == 0)
        {
            return "No vacancies available at the moment.\n";
        }

        string output = "";
        foreach (var vacancy in _vacancies)
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

    public bool VacancyExists(int id)
    {
        return _vacancies.Any(v => v.VacancyId == id);
    }
} 