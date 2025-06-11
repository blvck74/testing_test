using System;
using MonitoringShiftApp.ViewModels;
using MonitoringShiftApp.Models;
using MonitoringShiftApp.Data;
using Microsoft.EntityFrameworkCore;

namespace MonitoringShiftApp;

public class TestViewModel
{
    public static void TestNavigationLogic()
    {
        Console.WriteLine("Тестирование логики навигации Отдела Г...");
        
        // Создаем in-memory базу данных для тестирования
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
            
        using var context = new AppDbContext(options);
        
        // Создаем ViewModel
        var viewModel = new DepartmentGViewModel(context);
        
        // Проверяем начальное состояние
        Console.WriteLine($"Начальное состояние: {viewModel.CurrentState}");
        Console.WriteLine($"Показать темы: {viewModel.ShowTopicsView}");
        Console.WriteLine($"Показать подтемы: {viewModel.ShowSubtopicsView}");
        Console.WriteLine($"Показать список информации: {viewModel.ShowInformationListView}");
        Console.WriteLine($"Показать детали информации: {viewModel.ShowInformationDetailView}");
        Console.WriteLine($"Можно вернуться назад: {viewModel.CanGoBack}");
        Console.WriteLine($"Текущий заголовок: {viewModel.CurrentTitle}");
        
        // Создаем тестовые данные
        var topic = new DepartmentGTopic
        {
            Id = 1,
            Name = "Тестовая тема",
            Order = 1,
            CreatedAt = DateTime.UtcNow
        };
        
        var subtopic = new DepartmentGSubtopic
        {
            Id = 1,
            Name = "Тестовая подтема",
            TopicId = 1,
            Order = 1,
            CreatedAt = DateTime.UtcNow
        };
        
        var information = new DepartmentGInformation
        {
            Id = 1,
            Title = "Тестовая информация",
            Content = "Содержание тестовой информации",
            SubtopicId = 1,
            Order = 1,
            CreatedAt = DateTime.UtcNow
        };
        
        topic.Subtopics.Add(subtopic);
        subtopic.Information.Add(information);
        
        // Тестируем навигацию
        Console.WriteLine("\n--- Выбираем тему ---");
        viewModel.SelectTopicCommand.Execute(topic);
        Console.WriteLine($"Состояние: {viewModel.CurrentState}");
        Console.WriteLine($"Показать подтемы: {viewModel.ShowSubtopicsView}");
        Console.WriteLine($"Можно вернуться назад: {viewModel.CanGoBack}");
        Console.WriteLine($"Текущий заголовок: {viewModel.CurrentTitle}");
        
        Console.WriteLine("\n--- Выбираем подтему ---");
        viewModel.SelectSubtopicCommand.Execute(subtopic);
        Console.WriteLine($"Состояние: {viewModel.CurrentState}");
        Console.WriteLine($"Показать список информации: {viewModel.ShowInformationListView}");
        Console.WriteLine($"Текущий заголовок: {viewModel.CurrentTitle}");
        
        Console.WriteLine("\n--- Выбираем информацию ---");
        viewModel.SelectInformationCommand.Execute(information);
        Console.WriteLine($"Состояние: {viewModel.CurrentState}");
        Console.WriteLine($"Показать детали информации: {viewModel.ShowInformationDetailView}");
        Console.WriteLine($"Текущий заголовок: {viewModel.CurrentTitle}");
        
        Console.WriteLine("\n--- Возвращаемся назад ---");
        viewModel.GoBackCommand.Execute(null);
        Console.WriteLine($"Состояние: {viewModel.CurrentState}");
        Console.WriteLine($"Показать список информации: {viewModel.ShowInformationListView}");
        Console.WriteLine($"Текущий заголовок: {viewModel.CurrentTitle}");
        
        viewModel.GoBackCommand.Execute(null);
        Console.WriteLine($"Состояние: {viewModel.CurrentState}");
        Console.WriteLine($"Показать подтемы: {viewModel.ShowSubtopicsView}");
        Console.WriteLine($"Текущий заголовок: {viewModel.CurrentTitle}");
        
        viewModel.GoBackCommand.Execute(null);
        Console.WriteLine($"Состояние: {viewModel.CurrentState}");
        Console.WriteLine($"Показать темы: {viewModel.ShowTopicsView}");
        Console.WriteLine($"Можно вернуться назад: {viewModel.CanGoBack}");
        Console.WriteLine($"Текущий заголовок: {viewModel.CurrentTitle}");
        
        Console.WriteLine("\nТест завершен успешно!");
    }
}