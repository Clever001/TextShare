FROM mcr.microsoft.com/dotnet/sdk:10.0

# Установка базовых утилит, манов, protobuf и Node.js
RUN rm -f /etc/dpkg/dpkg.cfg.d/excludes && \
    apt-get update && \
    apt-get install -y --no-install-recommends \
    man-db \
    manpages \
    manpages-dev \
    golang-go \
    less \
    zsh \
    git \
    curl \
    btop \
    neofetch \
    tree \
    tldr \
    locales \
    nano \
    protobuf-compiler \
    ca-certificates \
    gnupg && \
    rm -rf /var/lib/apt/lists/*

# Установка Node.js (LTS версия 22 для стабильности)
RUN curl -fsSL https://deb.nodesource.com/setup_22.x | bash - && \
    apt-get install -y nodejs

# Установка gRPC утилит (grpcurl, grpcui)
RUN go install github.com/fullstorydev/grpcurl/cmd/grpcurl@latest && \
    go install github.com/fullstorydev/grpcui/cmd/grpcui@latest

# Настройка локали
RUN locale-gen ru_RU.UTF-8

ENV LANG=ru_RU.UTF-8 \
    LANGUAGE=ru_RU:ru \
    LC_ALL=ru_RU.UTF-8 \
    PATH="${PATH}:/root/go/bin"

# Настройка Zsh и окружения
RUN sh -c "$(curl -fsSL https://raw.githubusercontent.com/ohmyzsh/ohmyzsh/master/tools/install.sh)" "" --unattended && \
    sed -i 's/ZSH_THEME=.*/ZSH_THEME="robbyrussell"/' /root/.zshrc && \
    echo 'export PATH="$PATH:$HOME/go/bin"' >> /root/.zshrc && \
    git clone --depth 1 https://github.com/junegunn/fzf.git /root/.fzf && \
    /root/.fzf/install --all

WORKDIR /workspace

SHELL ["/bin/zsh", "-c"]

CMD ["/bin/zsh"]