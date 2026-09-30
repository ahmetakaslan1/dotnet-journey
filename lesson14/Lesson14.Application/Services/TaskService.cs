using AutoMapper;
using Lesson14.Domain;
using Lesson14.Application.DTOs;
using Lesson14.Application.Repositories;

namespace Lesson14.Application.Services;

public class TaskService
{
    private readonly IGenericRepository<TaskItem> _taskRepository;
    private readonly IMapper _mapper;

    public TaskService(IGenericRepository<TaskItem> taskRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    // YENİLİK: Sadece "Kendi" görevlerini ekleyebilir (Güvenlik Kalkanı)
    public async Task<TaskResponseDto> CreateTaskAsync(TaskCreateDto dto, int userId)
    {
        var taskItem = _mapper.Map<TaskItem>(dto);
        taskItem.UserId = userId; // Arka planda görevi, giren kişiye bağlıyoruz!

        await _taskRepository.AddAsync(taskItem);
        await _taskRepository.SaveChangesAsync();

        return _mapper.Map<TaskResponseDto>(taskItem);
    }

    // YENİLİK: Herkes sadece kendi görevlerini görebilir!
    public async Task<IEnumerable<TaskResponseDto>> GetMyTasksAsync(int userId)
    {
        // Generic Repository'nin nimetlerinden faydalanıyoruz (Şartlı sorgu)
        var myTasks = await _taskRepository.GetAllAsync(t => t.UserId == userId);
        return _mapper.Map<IEnumerable<TaskResponseDto>>(myTasks);
    }

    // Görev silme (Yine sadece kendine aitse silebilir)
    public async Task DeleteTaskAsync(int taskId, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
            throw new Exception("Görev bulunamadı!");

        if (task.UserId != userId)
            throw new Exception("Başkasının görevini silemezsiniz! Hacker mısın sen?");

        _taskRepository.Delete(task);
        await _taskRepository.SaveChangesAsync();
    }

    public async Task<TaskResponseDto> ToggleTaskCompletionAsync(int taskId, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
            throw new Exception("Görev bulunamadı!");

        if (task.UserId != userId)
            throw new Exception("Başkasına ait bir görevi güncelleyemezsiniz!");

        task.IsCompleted = !task.IsCompleted;
        
        _taskRepository.Update(task);
        await _taskRepository.SaveChangesAsync();

        return _mapper.Map<TaskResponseDto>(task);
    }
}
