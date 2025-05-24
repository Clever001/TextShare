using System.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;
using TextShareApi.Benchmarks.Contexts;
using TextShareApi.Benchmarks.Models;

namespace TextShareApi.Benchmarks.Benchmarks;

public class TextsLoadBenchmark {
    private DbContextOptions<PostgresContext> _contextBuilder = null!;
    private IMinioClient _minioClient = null!;
    private string _bucket = null!;
    [GlobalSetup]
    public async Task Setup() {
        // Подключение DbContext для работы Postgresql.
        var optionsBuilder = new DbContextOptionsBuilder<PostgresContext>();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        _contextBuilder = optionsBuilder.Options;
        var postgresContext = new PostgresContext(optionsBuilder.Options);
        
        // Проверка наличие текста в текущих директориях.
        var benchmarkConfig = configuration.GetSection("BenchmarkConfig");
        var root = benchmarkConfig.GetValue<string>("RootDirectory")
                                     ?? throw new InvalidOperationException();
        var textsDir = new DirectoryInfo(Path.Combine(root, "Benchmarks", "TextsExamples"));
        
        List<string> textsNames = ["LargeText.txt", "MediumText.txt", "SmallText.txt"];
        if (!textsDir.EnumerateFiles().Select(f => f.Name).All(n => textsNames.Contains(n))) {
            throw new NullReferenceException("Cannot find textsExamples");
        }
        
        // Вставка текстов в Postgresql.
        var smallText = new Text {
            Id = "small",
            Content = await File.ReadAllTextAsync(textsDir.GetFiles("SmallText.txt").First().FullName),
        };
        var mediumText = new Text {
            Id = "medium",
            Content = await File.ReadAllTextAsync(textsDir.GetFiles("MediumText.txt").First().FullName),
        };
        var largeText = new Text {
            Id = "large",
            Content = await File.ReadAllTextAsync(textsDir.GetFiles("LargeText.txt").First().FullName),
        };
        
        postgresContext.Texts.AddRange(smallText, mediumText, largeText);
        await postgresContext.SaveChangesAsync();
        
        // Подключение Minio
        var minioConfig = configuration.GetSection("MinioConfig");
        _minioClient = new MinioClient()
            .WithEndpoint(minioConfig.GetValue<string>("Endpoint")!)
            .WithCredentials(minioConfig.GetValue<string>("AccessKey")!, minioConfig.GetValue<string>("SecretKey")!)
            .WithSSL(false)
            .Build();
        
        _bucket = minioConfig.GetValue<string>("BucketName")!;
        bool bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucket));

        if (!bucketExists) {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucket));
        }
        
        // Сохранение текстов в Minio
        string textContent = await File.ReadAllTextAsync(textsDir.GetFiles("SmallText.txt").First().FullName);
        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(textContent));
        await _minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_bucket)
            .WithObject("small")
            .WithObjectSize(memoryStream.Length)
            .WithStreamData(memoryStream));
        textContent = await File.ReadAllTextAsync(textsDir.GetFiles("MediumText.txt").First().FullName);
        memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(textContent));
        await _minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_bucket)
            .WithObject("medium")
            .WithObjectSize(memoryStream.Length)
            .WithStreamData(memoryStream));
        textContent = await File.ReadAllTextAsync(textsDir.GetFiles("LargeText.txt").First().FullName);
        memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(textContent));
        await _minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_bucket)
            .WithObject("large")
            .WithObjectSize(memoryStream.Length)
            .WithStreamData(memoryStream));
    }
    
    [GlobalCleanup]
    public async Task Cleanup() {
        // Удаление текстов с РБД.
        var context = new PostgresContext(_contextBuilder);
        var texts = context.Texts.ToArray();
        
        context.Texts.RemoveRange(texts);
        await context.SaveChangesAsync();
        
        var removeArgs = new RemoveObjectArgs()
            .WithBucket(_bucket)
            .WithObject("small");
        await _minioClient.RemoveObjectAsync(removeArgs);
        removeArgs = new RemoveObjectArgs()
            .WithBucket(_bucket)
            .WithObject("medium");
        await _minioClient.RemoveObjectAsync(removeArgs);
        removeArgs = new RemoveObjectArgs()
            .WithBucket(_bucket)
            .WithObject("large");
        await _minioClient.RemoveObjectAsync(removeArgs);
    }
    
    [Benchmark]
    public async Task PostgresSmallTextLoad() {
        var context = new PostgresContext(_contextBuilder);
        string? content = (await context.Texts.FindAsync("small"))?.Content;
        
        if (content == null || content.Length < 5000) 
            throw new NullReferenceException("Cannot find text");
    }
    
    [Benchmark]
    public async Task PostgresMediumTextLoad() {
        var context = new PostgresContext(_contextBuilder);
        string? content = (await context.Texts.FindAsync("medium"))?.Content;
        
        if (content == null || content.Length < 75000) 
            throw new NullReferenceException("Cannot find text");
    }
    
    [Benchmark]
    public async Task PostgresLargeTextLoad() {
        var context = new PostgresContext(_contextBuilder);
        string? content = (await context.Texts.FindAsync("large"))?.Content;
        
        if (content == null || content.Length < 1500000) 
            throw new NullReferenceException("Cannot find text");
    }

    [Benchmark]
    public async Task MinioSmallTextLoad() {
        string result = "";
        var getObjectArgs = new GetObjectArgs()
            .WithBucket(_bucket)
            .WithObject("small")
            .WithCallbackStream(async (s, token) => {
                using var reader = new StreamReader(s);
                result = await reader.ReadToEndAsync(token);
            });
                    

        await _minioClient.GetObjectAsync(getObjectArgs);

        if (result == "" || result.Length < 5000)
            throw new NullReferenceException($"Cannot find text: \"{result}\"");
    }
    
    [Benchmark]
    public async Task MinioMediumTextLoad() {
        string result = "";
        var getObjectArgs = new GetObjectArgs()
            .WithBucket(_bucket)
            .WithObject("medium")
            .WithCallbackStream(async (s, token) => {
                using var reader = new StreamReader(s);
                result = await reader.ReadToEndAsync(token);
            });
                    

        await _minioClient.GetObjectAsync(getObjectArgs);

        if (result == "" || result.Length < 5000)
            throw new NullReferenceException($"Cannot find text: \"{result}\"");
    }
    
    [Benchmark]
    public async Task MinioLargeTextLoad() {
        string result = "";
        var getObjectArgs = new GetObjectArgs()
            .WithBucket(_bucket)
            .WithObject("large")
            .WithCallbackStream(async (s, token) => {
                using var reader = new StreamReader(s);
                result = await reader.ReadToEndAsync(token);
            });
                    

        await _minioClient.GetObjectAsync(getObjectArgs);

        if (result == "" || result.Length < 5000)
            throw new NullReferenceException($"Cannot find text: \"{result}\"");
    }
}